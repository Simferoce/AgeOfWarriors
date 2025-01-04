using Game.UI.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.UI.Windows
{
    public class WindowManager : Manager<WindowManager>
    {
        [SerializeField] private ColorRegistry colorRegistry;

        private List<Window> windows = new List<Window>();
        private Dictionary<Type, Window> windowPrefabs = new Dictionary<Type, Window>();
        private AsyncOperationHandle<IList<GameObject>> windowPrefabsHandle;

        public override IEnumerator InitializeAsync()
        {
            List<GameObject> windowPrefabs = new List<GameObject>();
            windowPrefabsHandle = Addressables.LoadAssetsAsync<GameObject>("Window", (GameObject gameObject) => LoadWindowPrefab(gameObject));
            yield return windowPrefabsHandle;
        }

        private void LoadWindowPrefab(GameObject prefab)
        {
            if (!prefab.TryGetComponent<Window>(out Window window))
            {
                Debug.LogError($"Unable to correctly load the window {prefab} because it does not have a {nameof(Window)} script.");
                return;
            }

            windowPrefabs.Add(window.GetType(), window);
        }

        private void OnDestroy()
        {
            if (windowPrefabsHandle.IsValid())
                Addressables.Release(windowPrefabsHandle);
        }

        public T GetWindow<T>()
            where T : Window
        {
            if (!windowPrefabs.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Unable to load the window with \"{typeof(T)}\" because there is no prefab corresponding.");
                return default;
            }

            bool isUnique = windowPrefabs[typeof(T)].IsUnique;

            T window = default;
            if (isUnique)
            {
                window = windows.OfType<T>().FirstOrDefault();

                if (window != null)
                    return window;
            }

            window = (T)GameObject.Instantiate(windowPrefabs[typeof(T)], this.transform);
            window.gameObject.SetActive(false);
            windows.Add(window);
            return window;
        }

        public Color GetColor(ColorRegistry.Identifiant identifiant)
        {
            return colorRegistry.GetColor(identifiant);
        }
    }
}
