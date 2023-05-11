using UnityEngine;

public class BallAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isAnimationPlaying;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAnimationPlaying = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isAnimationPlaying)
                Jump();
            else
                ResumeAnimation();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            FlyAway();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleAnimation();
        }
    }

    private void Jump()
    {
        animator.SetTrigger("JumpTrigger");
    }

    private void FlyAway()
    {
        animator.SetTrigger("FlyTrigger");
    }

    private void ToggleAnimation()
    {
        isAnimationPlaying = !isAnimationPlaying;
        animator.SetBool("IsPlaying", isAnimationPlaying);
    }

    private void ResumeAnimation()
    {
        isAnimationPlaying = true;
        animator.SetBool("IsPlaying", true);
    }
}
