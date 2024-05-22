using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{
    public GameObject start;
    public GameObject end;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public float speed;
    private float journeyLength;
    private float startTime;

    void Start()
    {
        if (start != null && end != null)
        {
            startPosition = start.transform.position;
            endPosition = end.transform.position;
            journeyLength = Vector3.Distance(startPosition, endPosition);
            startTime = Time.time;
        }
        else
        {
            Debug.LogError("Start y End deben estar asignados en el Inspector.");
        }
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.PingPong(fractionOfJourney, 1));
    }
}
