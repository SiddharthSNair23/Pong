using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public BallAudio ballAudio;
    public ParticleSystem collisionParticles;

    public float angle = 0.60f;
    public float moveSpeed = 1f;
    public float maxStartY = 4f;
    public float maxCollisionAngle = 45f;
    public float speedMultiplier = 1.1f;

    private float startX = 0f;

    private void Start()
    {
        GameManager.instance.onReset += ResetBall;
        GameManager.instance.gameUI.onStartGame += ResetBall;
    }

    private void ResetBall()
    {
        ResetBallPosition();
        Push();
    }

    private void Push()
    {
        Vector2 dir;
        if(Random.value < 0.5f)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }
        dir.y = Random.Range(-angle, angle);
        rb.linearVelocity = dir * moveSpeed;
        EmitParticle(32);
    }

    private void ResetBallPosition()
    {
        float posY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, posY);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>();
        if(scoreZone)
        {
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();
        if(paddle)
        {
            ballAudio.PlayPaddleSound();
            rb.linearVelocity *= speedMultiplier;
            if(rb.linearVelocity.magnitude > 20f)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * 20f;
            }
            EmitParticle(8);
            AdjustAngle(paddle, collision);
            GameManager.instance.screenShake.StartShake(Mathf.Sqrt(rb.linearVelocity.magnitude) * 0.02f, 0.075f);
        }

        Wall wall = collision.collider.GetComponent<Wall>();
        if(wall)
        {
            ballAudio.PlayWallSound();
            EmitParticle(8);
            GameManager.instance.screenShake.StartShake(0.033f, 0.033f);
        }
    }

    private void EmitParticle(int amount)
    {
        collisionParticles.Emit(amount);
    }

    private void AdjustAngle(Paddle paddle, Collision2D collision)
    {
        Vector2 median = Vector2.zero;
        foreach(ContactPoint2D point in collision.contacts)
        {
            median += point.point;
        }
        median /= collision.contactCount;
        
        float absoluteDistanceFromCenter = median.y - paddle.transform.position.y;
        float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / paddle.transform.localScale.y;

        int angleSign = paddle.IsLeftPaddle() ? 1 : -1;
        float angle = relativeDistanceFromCenter * maxCollisionAngle*angleSign;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 dir = paddle.IsLeftPaddle() ? Vector2.right : Vector2.left;
        Vector2 velocity = rot * dir * rb.linearVelocity.magnitude;
        rb.linearVelocity = velocity;
    }
}