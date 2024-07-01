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
    [SerializeField] private List<SceneActionMap> sceneActionList = new List<SceneActionMap>();

    private void Awake()
    {
        CustomSceneManager.SetScenes(sceneActionList);
        CustomSceneManager.LoadMainMenu();
    }

    public List<SceneActionMap> GetScenesList()
    {
        return sceneActionList;
    }
}
