using Game.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class Universe : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }

        public static Universe Instance { get; private set; }

        public event System.Action<Universe> OnLoaded;

        private List<Manager> managers = new List<Manager>();
        private AsyncOperationHandle<IList<GameObject>> managerPrefabsHandle;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is already an instance of {nameof(Universe)} in the scene at {Instance.transform.GetFullPath()}. Replacing it.");
                Destroy(Instance.gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public IEnumerator Start()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            List<GameObject> managerPrefabs = new List<GameObject>();
            managerPrefabsHandle = Addressables.LoadAssetsAsync<GameObject>("Manager", (GameObject gameObject) => managerPrefabs.Add(gameObject));
            yield return managerPrefabsHandle;

            foreach (GameObject managerPrefab in managerPrefabs)
            {
                try
                {
                    GameObject managerGameObject = GameObject.Instantiate(managerPrefab, this.transform);
                    if (!managerGameObject.TryGetComponent<Manager>(out Manager manager))
                    {
                        Debug.LogError($"Unable to correctly load the manager {managerGameObject} because it does not have a {nameof(Manager)} script.");
                        continue;
                    }

                    managers.Add(manager);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            foreach (Manager manager in managers)
                yield return manager.InitializeAsync();

            OnLoaded?.Invoke(this);
        }

        private void OnDestroy()
        {
            if (managerPrefabsHandle.IsValid())
                Addressables.Release(managerPrefabsHandle);
        }
    }
}
