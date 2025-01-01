using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game
{
    public class GameScene
    {
        public SceneDefinition SceneDefinition { get; private set; }

        private AsyncOperationHandle<SceneInstance> loadAsyncOperationHandle;
        private AsyncOperationHandle<SceneInstance> unloadAsyncOperationHandle;

        public GameScene(SceneDefinition sceneDefinition)
        {
            SceneDefinition = sceneDefinition;
        }

        public IEnumerator Load()
        {
            if (loadAsyncOperationHandle.IsValid() && loadAsyncOperationHandle.IsDone)
            {
                Debug.LogError($"The scene \"{SceneDefinition.name}\" is already loaded.");
                yield break;
            }

            loadAsyncOperationHandle = Addressables.LoadSceneAsync(SceneDefinition.Scene);
            yield return loadAsyncOperationHandle;

            if (loadAsyncOperationHandle.Status == AsyncOperationStatus.Failed)
                Debug.LogError($"The loading of the scene \"{SceneDefinition.name}\" failed.", SceneDefinition);
        }

        public IEnumerable Unload()
        {
            if (unloadAsyncOperationHandle.IsValid() && !unloadAsyncOperationHandle.IsDone)
            {
                Debug.LogError($"The scene \"{SceneDefinition.name}\" is already being unloaded.");
                yield break;
            }

            if (!loadAsyncOperationHandle.IsValid())
            {
                Debug.LogError($"The scene \"{SceneDefinition.name}\" has not been loaded.");
                yield break;
            }
            else if (loadAsyncOperationHandle.IsValid() && !loadAsyncOperationHandle.IsDone)
            {
                Debug.LogError($"The scene \"{SceneDefinition.name}\" is currently loading. Waiting to unload it.");
                yield return loadAsyncOperationHandle;
            }

            unloadAsyncOperationHandle = Addressables.UnloadSceneAsync(loadAsyncOperationHandle);
            yield return unloadAsyncOperationHandle;

            Addressables.Release(loadAsyncOperationHandle);
            Addressables.Release(unloadAsyncOperationHandle);
        }
    }
}
