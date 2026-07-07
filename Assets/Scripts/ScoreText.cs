using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Animator animator;

    public void Trigger()
    {
        animator.SetTrigger("Trigger");
    }

    public void SetScore(int value)
    {
        text.text = value.ToString();
    }
}