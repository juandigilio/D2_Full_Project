using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestCamera
{
    public static void SetCamera(GameObject cameraPoint, Camera mainCamera, float cameraHeight, float offsetZ)
    {
        Vector3 newPos = cameraPoint.transform.position;
        newPos.y += cameraHeight;
        newPos.z -= offsetZ;

        mainCamera.transform.position = newPos;

        mainCamera.transform.LookAt(cameraPoint.transform.position);
    }
}
