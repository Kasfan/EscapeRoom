using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Ground dropzone that works in multiplayer. This dropzone does not hold object, only places them in drop point.
    /// </summary>
    public class NetworkedDropZoneGround : NetworkedDropZone
    {
        /// <inheritdoc/>
        public override bool CanAccept(IGrabInteractable interactable) => interactable is NetworkedGrabInteractable;

        /// <inheritdoc/>
        public override void TryDropInteractable(IGrabInteractable interactable, Vector3 point)
        {
            if (interactable is NetworkedGrabInteractable networkedGrabbable)
            {
                TryDropInteractableRpc(networkedGrabbable.NetworkObject.NetworkObjectId, point);
                return;
            }

            interactable.Transform.parent = null;
            interactable.Transform.position = point;
        }
        
        /// <inheritdoc/>
        public override void TryDropInteractableRpc(ulong grabbableId, Vector3 point)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(grabbableId, out var objectToDrop);
            if (!objectToDrop)
                return;
            
            if (objectToDrop.TryRemoveParent())
            {
                objectToDrop.transform.position = point;
            }
        }
        
        /// <inheritdoc/>
        public override bool Empty => true;
        
        /// <inheritdoc/>
        public override IGrabInteractable DroppedInteractable => null;
    }
}