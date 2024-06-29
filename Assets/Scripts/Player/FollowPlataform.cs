using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlataform : MonoBehaviour
{
    //private Player player;
    private MovementBehaviour movementBehaviour;
    private Vector3 groundPosition;
    private Vector3 lastGroundPositon;
    private int groundID;
    private int lastGroundID;
    private float height;
    Quaternion actualRotation;
    Quaternion lastRotation;

    void Start()
    {
        movementBehaviour = GetComponent<MovementBehaviour>();

        GetObjectHeight();
    }

    private void OnDrawGizmos()
    {
        //playerObj = GetComponent<GameObject>();
        //Gizmos.DrawSphere(playerObj.transform.position, height / 4.0f);
    }

    void FixedUpdate()
    {
        if (movementBehaviour.IsGrounded())
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, height / 4.2f, -transform.up, out hit))
            {
                GameObject groundedIn = hit.collider.gameObject;

                groundID = groundedIn.GetInstanceID();
                groundPosition = groundedIn.transform.position;
                actualRotation = groundedIn.transform.rotation;

                //Debug.Log(groundID);

                if (groundID == lastGroundID)
                {
                    if (groundPosition != lastGroundPositon)
                    {
                        transform.position += groundPosition - lastGroundPositon;
                    }

                    if (actualRotation != lastRotation)
                    {
                        Vector3 newRotation = transform.rotation * (actualRotation.eulerAngles - lastRotation.eulerAngles);
                        transform.RotateAround(groundedIn.transform.position, Vector3.up, newRotation.y);
                    }
                }

                lastGroundID = groundID;
                lastGroundPositon = groundPosition;
                lastRotation = actualRotation;
            }
        }
        else if (!movementBehaviour.IsGrounded())
        {
            lastGroundID = -999;
            lastGroundPositon = Vector3.zero;
            lastRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    float GetObjectHeight()
    {
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            return collider.bounds.size.y;
        }
        else
        {
            Debug.LogError("El GameObject no tiene un componente Renderer.");
            return 0.0f;
        }
    }
}
