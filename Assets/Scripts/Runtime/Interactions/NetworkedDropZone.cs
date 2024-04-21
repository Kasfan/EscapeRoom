using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Simple drop zone that works in multiplayer 
    /// </summary>
    public class NetworkedDropZone: NetworkBehaviour, IDropZoneInteractable
    {
        /// <inheritdoc/>
        [field: SerializeField]
        [Tooltip("Hint message that will be shown, when player hovers object")]
        public virtual string InteractionTooltip { get; protected set; }

        [SerializeField] 
        [Tooltip("If true, dropped object will align with the attach point")] 
        protected bool matchPosition = true;
        
        [SerializeField] 
        [Tooltip("Transform that will be parent of the dropped object")] 
        protected Transform attachPoint;

        [SerializeField]
        [Tooltip("Objects the dropzone can accept. If empty, accepts all objects")]
        protected NetworkedGrabbable[] targetObjects;
        
        /// <inheritdoc/>
        public virtual bool Empty => attachPoint.childCount == 0;

        /// <inheritdoc/>
        public virtual IGrabInteractable DroppedInteractable => attachPoint.GetComponentInChildren<IGrabInteractable>();

        /// <inheritdoc/>
        public virtual bool CanAccept(IGrabInteractable interactable)
        {
            if(!Empty || interactable is not NetworkedGrabbable networkedGrabbable) // ignore not networked grabbable
                return false;

            if (targetObjects == null || targetObjects.Length == 0)
                return true;

            foreach (var target in targetObjects)
            {
                if (target == networkedGrabbable)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual void TryDropInteractable(IGrabInteractable interactable, Vector3 point)
        {
            if(!Empty || interactable is not NetworkedGrabbable networkedGrabbable) // ignore not networked grabbable
                return;

            TryDropInteractableRpc(networkedGrabbable.NetworkBehaviourId, point);
        }
        
        /// <summary>
        /// Call server to drop grabbable in this drop zone
        /// </summary>
        /// <param name="grabbableId">networked object id of the grabbable</param>
        /// <param name="point">drop position of the grabbable</param>
        [Rpc(SendTo.Server)]
        public virtual void TryDropInteractableRpc(ushort grabbableId, Vector3 point)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(grabbableId, out var objectToDrop);
            if (objectToDrop == null || objectToDrop.transform.parent != null) return;

            if (objectToDrop.TryGetComponent(out NetworkObject networkObject) 
                && networkObject.TrySetParent(attachPoint))
            {
                if (matchPosition)
                {
                    objectToDrop.transform.localPosition = Vector3.zero;
                    objectToDrop.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    objectToDrop.transform.position = point;
                }
            }
        }
    }
}