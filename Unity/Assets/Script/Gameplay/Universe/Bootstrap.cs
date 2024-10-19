using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [InitializeOnLoad]
    public static class Bootstrap
    {
        private static List<string> originalGameObjectsActivated;

        static Bootstrap()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                GameObject[] rootObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

                originalGameObjectsActivated = rootObjects.Where(x => x.activeSelf).Select(x => x.name).ToList();
                foreach (GameObject rootObject in rootObjects)
                    rootObject.SetActive(false);
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
                AddressableAssetEntry addressableAssetEntry = settings.groups.SelectMany(x => x.entries).FirstOrDefault(x => x.address.Equals("Universe"));
                if (addressableAssetEntry == null)
                {
                    Debug.LogError("Unable to find the Universe from Addressables.");
                    Activate();
                    return;
                }

                string assetPath = AssetDatabase.GUIDToAssetPath(addressableAssetEntry.guid);
                GameObject universePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (universePrefab == null)
                {
                    Debug.LogError($"Unable to load the asset at {assetPath} as a {nameof(GameObject)}");
                    Activate();
                    return;
                }

                GameObject universeGameObject = GameObject.Instantiate(universePrefab);
                if (!universeGameObject.TryGetComponent<Universe>(out Universe universe))
                {
                    Debug.LogError($"Unable to get the component {nameof(Universe)} from {universeGameObject}", universeGameObject);
                    Activate();
                    return;
                }

                universe.OnLoaded += UniverseOnLoaded;
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                Activate();
            }
        }

        private static void UniverseOnLoaded(Universe universe)
        {
            universe.OnLoaded -= UniverseOnLoaded;
            Activate();
        }

        private static void Activate()
        {
            foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects().Where(x => originalGameObjectsActivated.Contains(x.name)))
                gameObject.SetActive(true);
        }
    }
}