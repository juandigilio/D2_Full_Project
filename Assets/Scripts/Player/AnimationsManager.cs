using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private Animator animator;
    private MovementBehaviour movementBehaviour;
    private float landingSpeed;
    private float badLandingLimit = 10.0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        movementBehaviour = GetComponent<MovementBehaviour>();

        PrayBehaviour.OnAnimationPraying += Praying;
        JumpBehaviour.OnPlayerJumped += AnimateJump;
    }

    private void FixedUpdate()
    {
        CheckSpeed();
        CheckLanding();
        IsStuck();
    }

    private void CheckSpeed()
    {
        if (movementBehaviour.IsGrounded())
        {
            animator.SetFloat("verticalVelocity", 0.0f);
            animator.SetFloat("horizontalVelocity", movementBehaviour.rb.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("verticalVelocity", movementBehaviour.rb.velocity.y);

            if (movementBehaviour.rb.velocity.y < -2.0f)
            {
                animator.SetFloat("horizontalVelocity", 0.0f);
            }
        }
    }

    private void CheckLanding()
    {
        if (movementBehaviour.isLanding)
        {
            animator.SetTrigger("landed");
            animator.SetFloat("landingSpeed", landingSpeed);
            animator.ResetTrigger("jumped");

            player.SetAnimating(true);

            if (landingSpeed > badLandingLimit)
            {
                movementBehaviour.badLanded = true;
            }

            landingSpeed = 0.0f;

            movementBehaviour.isLanding = false;
        }
        else if (!movementBehaviour.IsGrounded())
        {
            if (movementBehaviour.rb.velocity.y < landingSpeed)
            {
                landingSpeed = movementBehaviour.rb.velocity.y;
            }
        }
    }

    private void Praying()
    {
        animator.SetTrigger("pray");
        player.SetAnimating(true);
    }

    private void IsStuck()
    {
        animator.SetBool("isStuck", movementBehaviour.IsStuck());

        if (movementBehaviour.IsStuck())
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", movementBehaviour.IsGrounded());
        }
    }

    private void AnimateJump()
    {
        animator.SetTrigger("jumped");
        animator.ResetTrigger("landed");
    }
}
