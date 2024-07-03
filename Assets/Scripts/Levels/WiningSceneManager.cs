using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiningSceneManager : MonoBehaviour
{
    float duration = 5f;
    float elapsedTime = 0f;

    private void Awake()
    {
        StartCoroutine(ShowPause());
    }

    private IEnumerator ShowPause()
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CustomSceneManager.ResetGame();
    }
}
