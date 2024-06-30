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
    [SerializeField] private static List<SceneActionMap> sceneActionList = new List<SceneActionMap>();

    public static List<SceneActionMap> GetScenesList()
    {
        return sceneActionList;
    }
}
