using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{
    public List<GameObject> points; 
    public float speed;             
    public bool requiresTrigger;    
    public bool stopAtLastPoint;    
    public bool rotatePlatform;     
    public float rotationDegrees;   

    private int currentPointIndex;  
    private Vector3 startPosition;  
    private Vector3 originalPosition; 
    private Vector3 endPosition;    
    private Quaternion originalRotation; 
    private Quaternion targetRotation;   
    private Quaternion currentTargetRotation; 
    private Vector3 returnStartPosition;
    private Quaternion returnStartRotation; 
    private float journeyLength;    
    private float startTime;        
    private bool isMoving;          
    private bool atLastPoint;       
    private bool returningToStart;  

    private Collider collider;

    void Start()
    {
        if (points != null && points.Count > 1)
        {
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            targetRotation = originalRotation * Quaternion.Euler(0, rotationDegrees, 0);
            currentPointIndex = 0;
            atLastPoint = false;
            returningToStart = false;

            collider = FindFirstObjectByType<Collider>();

            if (!requiresTrigger)
            {
                SetNextJourney();
                isMoving = true;
            }
        }
        else
        {
            Debug.LogError("Assign unles end and start");
        }
    }

    void Update()
    {
        if ((isMoving || returningToStart) && points != null && points.Count > 1)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            if (returningToStart)
            {
                transform.position = Vector3.Lerp(returnStartPosition, originalPosition, fractionOfJourney);
                if (rotatePlatform)
                {
                    transform.rotation = Quaternion.Lerp(returnStartRotation, originalRotation, fractionOfJourney);
                }

                if (fractionOfJourney >= 1f)
                {
                    returningToStart = false;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

                if (rotatePlatform)
                {
                    float totalFraction = ((float)currentPointIndex + fractionOfJourney) / (points.Count - 1);
                    transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, totalFraction);
                }

                if (fractionOfJourney >= 1f)
                {
                    if (stopAtLastPoint && currentPointIndex == points.Count - 1)
                    {
                        isMoving = false;
                        atLastPoint = true;
                    }
                    else
                    {
                        currentPointIndex = (currentPointIndex + 1) % points.Count;
                        SetNextJourney();
                    }
                }
            }
        }
    }

    private void SetNextJourney()
    {
        startPosition = transform.position;
        endPosition = points[currentPointIndex].transform.position;
        journeyLength = Vector3.Distance(startPosition, endPosition);
        startTime = Time.time;

        if (rotatePlatform)
        {
            float rotationFraction = (float)currentPointIndex / (points.Count - 1);
            currentTargetRotation = Quaternion.Lerp(originalRotation, targetRotation, rotationFraction);
        }
    }

    private void SetReturnJourney()
    {
        returnStartPosition = transform.position;
        returnStartRotation = transform.rotation;
        journeyLength = Vector3.Distance(returnStartPosition, originalPosition);
        startTime = Time.time;
        returningToStart = true;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter!!!!!");
        if (requiresTrigger && !isMoving && !returningToStart)
        {
            SetNextJourney();
            isMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (requiresTrigger)
        {
            if (isMoving || atLastPoint)
            {
                SetReturnJourney();
                atLastPoint = false;
            }
        }
    }
}

