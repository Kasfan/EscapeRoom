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
        private InputAction grabAction;

        [field: SerializeField]
        public Transform attachPoint { get; private set; }

        /// <inheritdoc/>
        public bool Holding => attachPoint.childCount > 0;

        /// <inheritdoc/>
        public IGrabInteractable HoldingObject => attachPoint.GetComponentInChildren<IGrabInteractable>();


        /// <inheritdoc/>
        public void ProcessInteraction(IGrabInteractable interactable)
        {
            if (grabAction.triggered)
            {
                interactable.OnGrabbed();
            }
        }
        
        /// <inheritdoc/>
        public void ProcessInteraction(IDropZoneInteractable dropZone, Vector3 interactionPoint)
        {
            if (grabAction.triggered)
            {
                var grabbable = HoldingObject;
                if (dropZone.CanAccept(grabbable))
                    dropZone.TryDropInteractable(grabbable, interactionPoint);
            }
        }
    }
}