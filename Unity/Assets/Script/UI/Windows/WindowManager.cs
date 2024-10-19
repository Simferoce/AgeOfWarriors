using Game.UI.Utilities;
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
        private AsyncOperationHandle<IList<GameObject>> windowPrefabsHandle;

        public override IEnumerator InitializeAsync()
        {
            List<GameObject> windowPrefabs = new List<GameObject>();
            windowPrefabsHandle = Addressables.LoadAssetsAsync<GameObject>("Window", (GameObject gameObject) => windowPrefabs.Add(gameObject));
            yield return windowPrefabsHandle;

            foreach (GameObject windowPrefab in windowPrefabs)
            {
                windowPrefab.SetActive(false);

                GameObject windowGameObject = GameObject.Instantiate(windowPrefab, this.transform);
                if (!windowGameObject.TryGetComponent<Window>(out Window window))
                {
                    Debug.LogError($"Unable to correctly load the window {windowGameObject} because it does not have a {nameof(Window)} script.");
                    continue;
                }

                windows.Add(window);
            }
        }

        private void OnDestroy()
        {
            if (windowPrefabsHandle.IsValid())
                Addressables.Release(windowPrefabsHandle);
        }

        public T GetWindow<T>()
            where T : Window
        {
            T window = windows.OfType<T>().FirstOrDefault();

            return window;
        }

        public Color GetColor(ColorRegistry.Identifiant identifiant)
        {
            return colorRegistry.GetColor(identifiant);
        }
    }
}
