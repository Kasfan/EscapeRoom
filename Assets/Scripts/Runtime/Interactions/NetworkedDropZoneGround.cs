using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Ground dropzone that works in multiplayer. This dropzone does not hold object, only places them in drop point.
    /// </summary>
    public class NetworkedDropZoneGround : NetworkedDropZone
    {
        /// <summary>
        /// This dropzone accepts everything by default, we do not need any target objects
        /// </summary>
        protected new readonly NetworkedGrabbable[] targetObjects;
        
        /// <inheritdoc/>
        public override bool CanAccept(IGrabInteractable interactable) => interactable is NetworkedGrabbable;

        /// <inheritdoc/>
        public override void TryDropInteractable(IGrabInteractable interactable, Vector3 point)
        {
            if(!Empty || interactable is not NetworkedGrabbable networkedGrabbable) // ignore not networked grabbable
                return;

            TryDropInteractableRpc(networkedGrabbable.NetworkBehaviourId, point);
        }
        
        /// <inheritdoc/>
        public override void TryDropInteractableRpc(ushort grabbableId, Vector3 point)
        {
            NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(grabbableId, out var objectToDrop);
            if (objectToDrop == null || objectToDrop.transform.parent != null) return;

            if (objectToDrop.TryGetComponent(out NetworkObject networkObject) 
                && networkObject.TrySetParent((Transform)null))
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