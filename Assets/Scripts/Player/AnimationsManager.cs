using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private Player player;
    private MovementBehaviour movementBehaviour;
    private float landingSpeed;
    private float badLandingLimit = 10.0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        movementBehaviour = GetComponent<MovementBehaviour>();
    }

    private void FixedUpdate()
    {
        CheckSpeed();
        CheckLanding();
        IsStuck();
        CheckJump();
    }

    public void CheckSpeed()
    {
        if (player.isGrounded)
        {
            player.animator.SetFloat("verticalVelocity", 0.0f);
            player.animator.SetFloat("horizontalVelocity", movementBehaviour.rb.velocity.magnitude);
        }
        else
        {
            player.animator.SetFloat("verticalVelocity", movementBehaviour.rb.velocity.y);

            if (movementBehaviour.rb.velocity.y < -2.0f)
            {
                player.animator.SetFloat("horizontalVelocity", 0.0f);
            }
        }
    }

    public void CheckLanding()
    {
        if (movementBehaviour.isLanding)
        {
            player.animator.SetTrigger("landed");
            player.animator.SetFloat("landingSpeed", landingSpeed);
            player.animator.ResetTrigger("jumped");

            player.isAnimating = true;

            if (landingSpeed > badLandingLimit)
            {
                movementBehaviour.badLanded = true;
            }

            landingSpeed = 0.0f;

            movementBehaviour.isLanding = false;
        }
        else if (!player.isGrounded)
        {
            if (movementBehaviour.rb.velocity.y < landingSpeed)
            {
                landingSpeed = movementBehaviour.rb.velocity.y;
            }
        }
    }

    public void IsStuck()
    {
        player.isStuck = movementBehaviour.rb.velocity.magnitude > -0.1f &&
                         movementBehaviour.rb.velocity.magnitude < 0.1f &&
                         !player.isGrounded;

        player.animator.SetBool("isStuck", player.isStuck);

        if (player.isStuck)
        {
            player.animator.SetBool("isGrounded", true);
        }
        else
        {
            player.animator.SetBool("isGrounded", player.isGrounded);
        }
    }

    public void CheckJump()
    {
        if (player.jumpedAnimation)
        {
            player.animator.SetTrigger("jumped");
            player.animator.ResetTrigger("landed");

            player.jumpedAnimation = false;
        }
    }
}
