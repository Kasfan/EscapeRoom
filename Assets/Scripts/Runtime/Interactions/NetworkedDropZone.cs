using EscapeRoom.QuestLogic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;

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
        [RequireInterface(typeof(IGrabInteractable))]
        [Tooltip("Objects the dropzone can accept. If empty, accepts all objects")]
        protected Object[] targetObjects;
        
        /// <inheritdoc/>
        public virtual bool Empty => DroppedInteractable == null;

        /// <inheritdoc/>
        public virtual IGrabInteractable DroppedInteractable => transform.GetComponentInChildren<IGrabInteractable>();

        /// <inheritdoc/>
        public virtual bool CanAccept(IGrabInteractable interactable)
        {
            if(!Empty) // ignore not networked grabbable
                return false;

            if (targetObjects == null || targetObjects.Length == 0)
                return true;

            foreach (var target in targetObjects)
            {
                if ((IGrabInteractable)target == interactable)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public virtual void TryDropInteractable(IGrabInteractable interactable, Vector3 point)
        {
            if (!Empty) // ignore not networked grabbable
                return;

            if (interactable is NetworkedGrabInteractable networkedGrabbable)
            {
                TryDropInteractableRpc(networkedGrabbable.NetworkObject.NetworkObjectId, point);
                return;
            }
            
            interactable.Transform.parent = transform;
            interactable.Transform.localPosition = transform.InverseTransformPoint(attachPoint.position);;
            interactable.Transform.localRotation = Quaternion.Inverse(attachPoint.rotation) * transform.rotation;
        }

        /// <summary>
        /// Call server to drop grabbable in this drop zone
        /// </summary>
        /// <param name="grabbableId">networked object id of the grabbable</param>
        /// <param name="point">drop position of the grabbable</param>
        [Rpc(SendTo.Server)]
        public virtual void TryDropInteractableRpc(ulong grabbableId, Vector3 point)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(grabbableId, out var objectToDrop);
            if (!objectToDrop)
                return;

            if (objectToDrop.TrySetParent(transform))
            {
                if (matchPosition)
                {
                    objectToDrop.transform.localPosition = transform.InverseTransformPoint(attachPoint.position);;
                    objectToDrop.transform.localRotation = Quaternion.Inverse(attachPoint.rotation) * transform.rotation;;
                }
                else
                {
                    objectToDrop.transform.position = point;
                }
            }
        }
        
        
        /// <summary>
        /// Condition that check if dropzone is empty or not
        /// </summary>
        [System.Serializable]
        public class DropZoneEmpty: Condition
        {
            [SerializeField] 
            [RequireInterface(typeof(IDropZoneInteractable))]
            private Object dropZone;

            public override bool IsTrue
            {
                get
                {
                    if (dropZone == null)
                        return false;
                    var empty = ((IDropZoneInteractable)dropZone).Empty;
                    return invert? !empty: empty;
                }
            }
        }
    }
}