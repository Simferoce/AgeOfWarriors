using Game.Extensions;
using System.Collections;
using UnityEngine;

namespace Game
{
    public abstract class Manager : MonoBehaviour
    {
        public virtual IEnumerator InitializeAsync()
        {
            yield break;
        }
    }

    public abstract class Manager<T> : Manager
        where T : Manager<T>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Instance = null;
        }

        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"There is already an instance of {nameof(T)} in the scene at {Instance.transform.GetFullPath()}. Replacing it.");
                Destroy(Instance.gameObject);
            }

            Instance = (T)this;
        }
    }
}
