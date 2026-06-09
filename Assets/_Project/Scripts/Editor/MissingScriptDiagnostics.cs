#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterTreasureHunt.EditorTools
{
    public static class MissingScriptDiagnostics
    {
        [MenuItem("Tools/Diagnostics/Find Missing Scripts In Open Scenes")]
        private static void FindMissingScriptsInOpenScenes()
        {
            List<string> results = new List<string>();

            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                Scene scene = SceneManager.GetSceneAt(sceneIndex);
                if (!scene.isLoaded)
                {
                    continue;
                }

                foreach (GameObject rootObject in scene.GetRootGameObjects())
                {
                    CollectMissingScripts(rootObject, scene.path, results);
                }
            }

            ReportResults("open scenes", results);
        }

        [MenuItem("Tools/Diagnostics/Find Missing Scripts In Prefabs")]
        private static void FindMissingScriptsInPrefabs()
        {
            List<string> results = new List<string>();
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

            try
            {
                for (int index = 0; index < prefabGuids.Length; index++)
                {
                    string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuids[index]);
                    GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

                    try
                    {
                        CollectMissingScripts(prefabRoot, prefabPath, results);
                    }
                    finally
                    {
                        PrefabUtility.UnloadPrefabContents(prefabRoot);
                    }
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[MissingScriptDiagnostics] Failed while scanning prefabs: {exception.Message}");
                throw;
            }

            ReportResults("prefabs", results);
        }

        private static void CollectMissingScripts(GameObject gameObject, string ownerPath, List<string> results)
        {
            int missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
            if (missingCount > 0)
            {
                string hierarchyPath = GetHierarchyPath(gameObject.transform);
                results.Add($"{ownerPath} -> {hierarchyPath} ({missingCount} missing component{(missingCount == 1 ? string.Empty : "s")})");
                Debug.LogWarning($"[MissingScriptDiagnostics] {ownerPath} -> {hierarchyPath} has {missingCount} missing component{(missingCount == 1 ? string.Empty : "s")}.", gameObject);
            }

            foreach (Transform child in gameObject.transform)
            {
                CollectMissingScripts(child.gameObject, ownerPath, results);
            }
        }

        private static string GetHierarchyPath(Transform transform)
        {
            if (transform == null)
            {
                return "<null>";
            }

            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = $"{transform.name}/{path}";
            }

            return path;
        }

        private static void ReportResults(string scopeName, List<string> results)
        {
            if (results.Count == 0)
            {
                Debug.Log($"[MissingScriptDiagnostics] No missing scripts found in {scopeName}.");
                return;
            }

            Debug.LogWarning($"[MissingScriptDiagnostics] Found {results.Count} object(s) with missing scripts in {scopeName}:\n- {string.Join("\n- ", results)}");
        }
    }
}
#endif
