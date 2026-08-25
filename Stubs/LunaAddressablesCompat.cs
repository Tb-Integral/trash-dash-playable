using System;
using System.Collections;
using UnityEngine;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
    public struct AsyncOperationHandle : IEnumerator
    {
        public GameObject Result;
        public bool IsDone { get { return true; } }
        public object Current { get { return Result; } }
        public event Action<AsyncOperationHandle> Completed;
        public bool MoveNext() { return false; }
        public void Reset() { }
    }

    public struct AsyncOperationHandle<T> : IEnumerator
    {
        public T Result;
        public bool IsDone { get { return true; } }
        public object Current { get { return Result; } }
        public event Action<AsyncOperationHandle<T>> Completed;
        public bool MoveNext() { return false; }
        public void Reset() { }

        public static implicit operator AsyncOperationHandle(AsyncOperationHandle<T> handle)
        {
            GameObject go = handle.Result as GameObject;
            return new AsyncOperationHandle { Result = go };
        }
    }
}

namespace UnityEngine.AddressableAssets
{
    [Serializable]
    public class AssetReference
    {
        public string AssetGUID;
        public string SubObjectName;
        public object RuntimeKey { get { return AssetGUID; } }
        public UnityEngine.Object Asset { get { return null; } }

        public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> InstantiateAsync()
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>);
        }

        public UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> InstantiateAsync(Vector3 position, Quaternion rotation)
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject>);
        }
    }

    public static class Addressables
    {
        public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle InstantiateAsync(object key)
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle);
        }

        public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle InstantiateAsync(object key, Vector3 position, Quaternion rotation)
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle);
        }

        public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle LoadAssetAsync<T>(object key)
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle);
        }

        public static UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle LoadAssetsAsync<T>(object key, Action<T> callback)
        {
            return default(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle);
        }

        public static void ReleaseInstance(GameObject obj) { }
    }
}