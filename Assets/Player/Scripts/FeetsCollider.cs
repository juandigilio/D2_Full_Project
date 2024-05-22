using UnityEngine;

public class FeetsCollider : MonoBehaviour
{
    public GameObject playerObj;
    private Rigidbody rb;

    public bool isInTrigger = false;
    public float fallingSpeed;

    private void Start()
    {
        rb = playerObj.GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.Log("Pie sin rigibody");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
        fallingSpeed = rb.velocity.y;
    }

    void OnTriggerStay(Collider other)
    {
        isInTrigger = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }
}
