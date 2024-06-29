using UnityEngine;

public class FeetsCollider : MonoBehaviour
{
    public Rigidbody rb;

    public bool isInTrigger = false;
    public float fallingSpeed;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            Debug.Log("Pie sin rigibody");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
        fallingSpeed = rb.velocity.y;
        Debug.Log("entando al suelo");
    }

    void OnTriggerStay(Collider other)
    {
        isInTrigger = true;
        Debug.Log("tocando");
    }

    void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
        Debug.Log("saliendo del suelo");
    }
}
