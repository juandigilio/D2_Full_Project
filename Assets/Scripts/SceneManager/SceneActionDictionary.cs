using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneActionMap
{
    public string sceneName;
    public string actionName;
}

public class SceneActionDictionary : MonoBehaviour
{
    [SerializeField] private SceneActionMap mainScene;
    [SerializeField] private SceneActionMap mainMenu;
    [SerializeField] private List<SceneActionMap> sceneActionList = new List<SceneActionMap>();
    [SerializeField] private SceneActionMap winingScene;

    /// <summary>
    /// Get scene list from unity editor and load main menu
    /// </summary>
    private void Awake()
    {
        CustomSceneManager.SetScenes(mainScene, mainMenu, sceneActionList, winingScene);
        CustomSceneManager.LoadMainMenu();
    }
}
