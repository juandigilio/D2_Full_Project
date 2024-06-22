using UnityEngine;

public class PushRigisbodys : MonoBehaviour
{
    public float pushForce = 2.0f;
    private float targetMass;
    private Rigidbody player;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody body = collision.collider.attachedRigidbody;

        if (!body || body.isKinematic)
        {
            return;
        }

        Vector3 velocity = player.velocity;

        if (velocity.y < -0.1)
        {
            return;
        }

        targetMass = body.mass;

        Vector3 pushDir = new Vector3(velocity.x, 0.0f, velocity.z);

        body.velocity = pushDir * pushForce / targetMass;
    } 
}
