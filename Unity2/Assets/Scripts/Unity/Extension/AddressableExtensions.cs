using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace AgeOfWarriors.Unity
{

    public static class AddressablesExtensions
    {
        public class AddressablesHandle<T> : IDisposable
        {
            private Dictionary<string, AsyncOperationHandle<T>> operationDictionary = new Dictionary<string, AsyncOperationHandle<T>>();

            public IEnumerable<T> Result => operationDictionary.Values.Select(x => x.Result);

            public AddressablesHandle(IList<IResourceLocation> locations)
            {
                foreach (IResourceLocation location in locations)
                {
                    AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(location);
                    operationDictionary.Add(location.PrimaryKey, handle);
                }
            }

            public void Add(string key, AsyncOperationHandle<T> handle)
            {
                operationDictionary.Add(key, handle);
            }

            public TaskAwaiter<T[]> GetAwaiter()
            {
                var tasks = operationDictionary.Values
                                    .Select(handle => handle.Task)
                                    .ToArray();

                return Task.WhenAll(tasks).GetAwaiter();
            }

            public void Dispose()
            {
                foreach (var item in operationDictionary)
                    Addressables.Release(item.Value);
            }
        }

        public static async Task<AddressablesHandle<T>> LoadAssetsAsync<T>(string keys)
        {
            AsyncOperationHandle<IList<IResourceLocation>> asyncOperationHandle = Addressables.LoadResourceLocationsAsync(keys);
            await AwaitHandleCompletion(asyncOperationHandle);

            AddressablesHandle<T> handle = new AddressablesHandle<T>(asyncOperationHandle.Result);
            await handle;

            Addressables.Release(asyncOperationHandle);
            return handle;
        }

        private static Task AwaitHandleCompletion<T>(AsyncOperationHandle<T> handle)
        {
            var tcs = new TaskCompletionSource<bool>();

            handle.Completed += completedHandle =>
            {
                if (completedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(new System.Exception($"Failed to load assets: {completedHandle.OperationException?.Message}"));
                }
            };

            return tcs.Task;
        }
    }
}