using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMovement : MonoBehaviour
{
    public List<GameObject> points; // Lista de puntos a seguir
    public float speed;             // Velocidad del movimiento
    public bool requiresTrigger;    // Si es verdadero, el movimiento inicia solo con contacto
    public bool stopAtLastPoint;    // Si es verdadero, se detiene en el último punto hasta que el jugador deje de tener contacto
    public bool rotatePlatform;     // Si es verdadero, la plataforma rota
    public float rotationDegrees;   // Grados de rotación total

    private int currentPointIndex;  // Índice del punto actual en la lista
    private Vector3 startPosition;  // Posición de inicio del movimiento actual
    private Vector3 originalPosition; // Posición inicial del objeto
    private Vector3 endPosition;    // Posición de destino del movimiento actual
    private Quaternion originalRotation; // Rotación inicial del objeto
    private Quaternion targetRotation;   // Rotación objetivo del objeto
    private Quaternion currentTargetRotation; // Rotación objetivo actual durante el movimiento
    private Vector3 returnStartPosition; // Posición de inicio del regreso al original
    private Quaternion returnStartRotation; // Rotación de inicio del regreso al original
    private float journeyLength;    // Longitud del viaje actual
    private float startTime;        // Tiempo de inicio del viaje actual
    private bool isMoving;          // Estado del movimiento
    private bool atLastPoint;       // Estado si está en el último punto
    private bool returningToStart;  // Estado de regreso a la posición inicial

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
            Debug.LogError("Debes asignar al menos dos puntos en la lista de puntos en el Inspector.");
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
        isMoving = false; // Detenemos el movimiento normal para iniciar el regreso
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
                atLastPoint = false; // Reseteamos atLastPoint
            }
        }
    }
}

