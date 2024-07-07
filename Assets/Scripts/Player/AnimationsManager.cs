using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Player player;
    private MovementBehaviour movementBehaviour;
    private PlayerSounds playerSounds;
    private float landingSpeed;
    [SerializeField] private float badLandingLimit = 10.0f;
    [SerializeField] private float nonLandingLimit = 5.0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        movementBehaviour = GetComponent<MovementBehaviour>();
        playerSounds = GetComponent<PlayerSounds>();

        PrayBehaviour.OnAnimationPraying += Praying;
        JumpBehaviour.OnPlayerJumped += AnimateJump;

        //animator.SetFloat("verticalVelocity", -0.2f);
        //animator.SetBool("isGrounded", false);
    }

    private void OnDisable()
    {
        PrayBehaviour.OnAnimationPraying -= Praying;
        JumpBehaviour.OnPlayerJumped -= AnimateJump;
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
            animator.SetFloat("horizontalVelocity", movementBehaviour.Displacement().magnitude);
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetFloat("verticalVelocity", movementBehaviour.Velocity().y); 
            animator.SetBool("isGrounded", false);
        }
    }

    private void CheckLanding()
    {
        if (movementBehaviour.IsLanding())
        {
            //animator.SetTrigger("landed");
            animator.SetFloat("landingSpeed", landingSpeed);
            animator.ResetTrigger("jumped");

            if (landingSpeed < -5)
            {
                player.SetAnimating(true);
            }
            

            if (landingSpeed > badLandingLimit)
            {
                movementBehaviour.BadLanded(true);
                playerSounds.PlayBadLandSound();
            }

            landingSpeed = 0.0f;

            movementBehaviour.IsLanding(false);
        }
        else if (!movementBehaviour.IsGrounded())
        {
            animator.SetBool("isGrounded", false);

            if (movementBehaviour.Velocity().y < landingSpeed)
            {
                landingSpeed = movementBehaviour.Velocity().y;
            }
        }
    }

    private void Praying()
    {
        playerSounds.PlayPraySound();
        animator.SetTrigger("pray");
        player.SetAnimating(true);
    }

    private void IsStuck()
    {
        animator.SetBool("isStuck", movementBehaviour.IsStuck());

        if (movementBehaviour.IsStuck())
        {
            animator.SetBool("isGrounded", true);
            //Debug.Log("is grounded");
            //Debug.Log("is stuck: " + movementBehaviour.IsStuck());
        }
        else
        {
            animator.SetBool("isGrounded", movementBehaviour.IsGrounded());
        }
    }

    private void AnimateJump()
    {
        playerSounds.PlayJumpSound();
        animator.SetTrigger("jumped");
        animator.ResetTrigger("landed");
    }
}
