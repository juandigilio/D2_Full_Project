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
    [SerializeField] private SceneActionMap mainMenu;
    [SerializeField] private List<SceneActionMap> sceneActionList = new List<SceneActionMap>();

    /// <summary>
    /// Get scene list from unity editor and load main menu
    /// </summary>
    private void Awake()
    {
        CustomSceneManager.SetScenes(sceneActionList);
        CustomSceneManager.LoadScene(mainMenu.sceneName);
    }

    /// <summary>
    /// Get scene list from unity editor
    /// </summary>
    public List<SceneActionMap> GetScenesList()
    {
        return sceneActionList;
    }
}
