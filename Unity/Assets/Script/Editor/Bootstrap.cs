using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Game
{
    [InitializeOnLoad]
    public static class Bootstrap
    {
        static Bootstrap()
        {
            string[] guids = AssetDatabase.FindAssets("t:scene 00-Empty");
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/_Game/Level/_Common/Scene/00-Empty.unity");
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                string[] guids = AssetDatabase.FindAssets("t:prefab Universe");
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                GameObject universePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Game/Managers/Universe.prefab");
                GameObject universeGameObject = GameObject.Instantiate(universePrefab);
                Universe universe = universeGameObject.GetComponent<Universe>();
                universe.OnLoaded += Universe_OnLoaded;
            }
        }

        private static void Universe_OnLoaded(Universe universe)
        {
            universe.OnLoaded -= Universe_OnLoaded;
            GameFlowManager.Instance.LoadMainMenu();
        }
    }
}