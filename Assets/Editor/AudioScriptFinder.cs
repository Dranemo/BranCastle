using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AudioScriptFinder : EditorWindow
{
    [MenuItem("Tools/Find All AudioSources")]
    public static void ShowWindow()
    {
        GetWindow<AudioScriptFinder>("Find All AudioSources");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find All AudioSources"))
        {
            FindAllAudioSources();
        }
    }

    private void FindAllAudioSources()
    {
        List<AudioSource> audioSources = new List<AudioSource>();

        // Find AudioSources in all active scenes
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                var scenePath = scene.path;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (sceneAsset != null)
                {
                    var sceneObjects = EditorUtility.CollectDependencies(new Object[] { sceneAsset });
                    foreach (var obj in sceneObjects)
                    {
                        if (obj is GameObject go)
                        {
                            audioSources.AddRange(go.GetComponentsInChildren<AudioSource>(true));
                        }
                    }
                }
            }
        }

        // Find AudioSources in all prefabs
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                audioSources.AddRange(prefab.GetComponentsInChildren<AudioSource>(true));
            }
        }

        // Log the results
        //////Debug.Log($"Found {audioSources.Count} AudioSources in the project.");
        foreach (var audioSource in audioSources)
        {
            //////Debug.Log($"AudioSource found in: {audioSource.gameObject.name}", audioSource.gameObject);
        }
    }
}
