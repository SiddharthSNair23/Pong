using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Rigidbody2D rb;
    public int id;
    public float moveSpeed = 2f;
    private float aiDeadzone = 1f;

    private Vector3 startPosition;
    private bool canMove = false;
    private int direction = 0;
    private float moveSpeedMultiplier = 1f;

    private void Start()
    {
        startPosition = transform.position;
        GameManager.instance.onReset += ResetPosition;
        GameManager.instance.gameUI.onStartGame += StartMoving;
    }

    private void StartMoving()
    {
        canMove = true;
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        if(id == 2 && GameManager.instance.IsPlayer2AI())
        {
            MoveAI();
        }
        else
        {
            float movement = ProcessInput();
            Move(movement);
        }
    }

    private void MoveAI()
    {
        Vector2 ballPos = GameManager.instance.ball.transform.position;
        if(Mathf.Abs(ballPos.y - transform.position.y) > aiDeadzone)
        {
            direction = ballPos.y > transform.position.y ? 1 : -1;
        }

        if(Random.value < 0.01f)
        {
            moveSpeedMultiplier = Random.Range(0.5f, 1.5f);
        }
        Move(direction);
    }

    private float ProcessInput()
    {
        float movement = 0f;
        switch(id)
        {
            case 1:
                movement = Input.GetAxis("MovePlayer1");
                break;

            case 2:
                movement = Input.GetAxis("MovePlayer2");
                break;
        }

        return movement;
    }

    private void Move(float movement)
    {
        Vector2 velo = rb.linearVelocity;
        velo.y = moveSpeed * moveSpeedMultiplier * movement;
        rb.linearVelocity = velo;
    }

    public float GetHeight()
    {
        return transform.localScale.y;
    }

    public bool IsLeftPaddle()
    {
        return id == 1;
    }
}
