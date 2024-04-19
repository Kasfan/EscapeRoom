using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace EscapeRoom.SceneManagement
{
    /// <summary>
    /// Loads and destroys addressable prefab to provide dynamically loaded scene mechanics.
    /// This script maintains the asset memory allocation.
    /// </summary>
    public class DynamicContentLoader: MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Reference of a prefab, that should be loaded")]
        private AssetReferenceGameObject prefabToLoad;

        [SerializeField]
        [Tooltip("If the the object should be destroyed when unloaded. If not the object will be disabled.")]
        private bool destroyOnUnload;
        
        private AsyncOperationHandle<GameObject>? loadingOp;
        
        /// <summary>
        /// Invoked whe loaded state of the prefab changes
        /// </summary>
        public event Action LoadedStateChanged;

        /// <summary>
        /// Loads the prefab from addressable
        /// </summary>
        public void Load()
        {
            if (loadingOp.HasValue && loadingOp.Value.IsValid())
            {
                loadingOp.Value.Result.SetActive(true);
                return;   
            }

            if (loadingOp.HasValue &&!loadingOp.Value.IsValid())
                Addressables.ReleaseInstance(loadingOp.Value);
            
            loadingOp =  prefabToLoad.InstantiateAsync(transform);
            loadingOp.Value.Completed += handle =>
            {
                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Could not load room prefab {prefabToLoad.AssetGUID}, {handle.OperationException}");
                    return;
                }
                LoadedStateChanged?.Invoke();
            };
        }

        /// <summary>
        /// Unloads the prefab.
        /// If <see cref="destroyOnUnload"/> flag is selected destroys the instantiated object
        /// and unloads the prefab from memory, disables  the instantiated object otherwise
        /// </summary>
        public void Unload()
        {
            if (!loadingOp.HasValue) 
                return;
            
            if (destroyOnUnload)
            {
                Addressables.ReleaseInstance(loadingOp.Value);
                LoadedStateChanged?.Invoke();
                loadingOp = null;
                return;
            }

            loadingOp.Value.Result.SetActive(false);
        }
        
    }
}