using UnityEngine;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        LevelManager.onDeathZone += ResetLevel;
    }

    private void OnDisable()
    {
        LevelManager.onDeathZone -= ResetLevel;
    }

    private void ResetLevel()
    {
        CustomSceneManager.ResetLevel();
    }
}
