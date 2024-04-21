using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactor that grabs a grabbable object when input action is triggered
    /// or drops already grabbed object on the same action
    /// </summary>
    public class GrabInteractor: NetworkBehaviour, IGrabInteractor
    {
        [SerializeField] 
        private InputActionReference grabAction;

        [field: SerializeField]
        public Transform attachPoint { get; private set; }

        /// <inheritdoc/>
        public bool Holding => HoldingObject != null;

        /// <inheritdoc/>
        public IGrabInteractable HoldingObject => GetComponentInChildren<IGrabInteractable>();


        /// <inheritdoc/>
        public void ProcessInteraction(IGrabInteractable interactable)
        {
            if (!grabAction.action.triggered || Holding)
                return;
            
            if (interactable is NetworkBehaviour networkGrabInteractable)
            {
                TryGrabRpc(networkGrabInteractable.NetworkObject.NetworkObjectId);
                return;
            }

            interactable.Transform.parent = transform;
            interactable.Transform.localPosition = transform.InverseTransformPoint(attachPoint.position);
            
        }

        /// <summary>
        /// Call server to grab an object
        /// </summary>
        /// <param name="grabbableId">networked object id of the grabbable</param>
        [Rpc(SendTo.Owner)]
        public virtual void TryGrabRpc(ulong grabbableId)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(grabbableId, out var objectToGrab);
            if (!objectToGrab)
                return;

            if (objectToGrab.TrySetParent(transform))
            {
                objectToGrab.transform.localPosition = transform.InverseTransformPoint(attachPoint.position);
            }
        }


        /// <inheritdoc/>
        public void ProcessInteraction(IDropZoneInteractable dropZone, Vector3 interactionPoint)
        {
            if (!grabAction.action.triggered)
                return;
            
            var grabbable = HoldingObject;
            if (dropZone.CanAccept(grabbable))
                dropZone.TryDropInteractable(grabbable, interactionPoint);
        }
    }
}