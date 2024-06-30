//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SceneActionDictionary))]
//public class SceneActionDictionaryEditor : Editor
//{
//    private SceneActionDictionary sceneActionDictionary;

//    private void OnEnable()
//    {
//        sceneActionDictionary = (SceneActionDictionary)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        EditorGUILayout.Space();
//        EditorGUILayout.LabelField("Static Scene Action List", EditorStyles.boldLabel);

//        // Mostrar la lista estática
//        if (SceneActionDictionary.sceneActionList == null)
//        {
//            SceneActionDictionary.sceneActionList = new List<SceneActionMap>();
//        }

//        for (int i = 0; i < SceneActionDictionary.sceneActionList.Count; i++)
//        {
//            EditorGUILayout.BeginHorizontal();
//            SceneActionDictionary.sceneActionList[i].sceneName = EditorGUILayout.TextField("Scene Name", SceneActionDictionary.sceneActionList[i].sceneName);
//            SceneActionDictionary.sceneActionList[i].actionName = EditorGUILayout.TextField("Action Name", SceneActionDictionary.sceneActionList[i].actionName);

//            if (GUILayout.Button("Remove", GUILayout.Width(70)))
//            {
//                SceneActionDictionary.sceneActionList.RemoveAt(i);
//            }

//            EditorGUILayout.EndHorizontal();
//        }

//        if (GUILayout.Button("Add New Scene Action"))
//        {
//            SceneActionDictionary.sceneActionList.Add(new SceneActionMap());
//        }

//        // Guardar cambios
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(target);
//        }
//    }
//}
