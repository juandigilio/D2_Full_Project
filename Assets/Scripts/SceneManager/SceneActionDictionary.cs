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
    public List<SceneActionMap> sceneActionList = new List<SceneActionMap>();

    private Dictionary<string, string> sceneActionDict;

    void Awake()
    {
        sceneActionDict = new Dictionary<string, string>();

        foreach (SceneActionMap item in sceneActionList)
        {
            if (!sceneActionDict.ContainsKey(item.sceneName))
            {
                sceneActionDict.Add(item.sceneName, item.actionName);
            }
        }
    }

    public string GetActionName(string sceneName)
    {
        if (sceneActionDict.TryGetValue(sceneName, out string actionName))
        {
            return actionName;
        }
        else
        {
            Debug.LogWarning("Scene name not found in dictionary: " + sceneName);
            return null;
        }
    }
}
