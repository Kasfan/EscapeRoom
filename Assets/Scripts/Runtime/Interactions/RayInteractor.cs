using EscapeRoom.UI;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    public class RayInteractor: MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("Layer mask for raycast")] 
        private LayerMask rayLayers;
        
        [SerializeField] 
        [Tooltip("Transform from which ray will originate forward")]
        private Transform rayOrigin;

        [SerializeField] 
        [Tooltip("UI of raycast interactor")]
        private RayInteractorUI ui; 
        
        [SerializeField] 
        private float interactionDistance;
        
        private IGrabInteractor grabInteractor;
        private IInteractor<IActivateInteractable> activateInteractor;

        private void Awake()
        {
            grabInteractor = GetComponent<IGrabInteractor>();
            activateInteractor = GetComponent<IInteractor<IActivateInteractable>>();
        }

        private void FixedUpdate()
        {
            var (interactable, hitPoint) = CastRay();

            if (interactable is IGrabInteractable grabInteractable)
            {
                OnGrabInteractableHit(grabInteractable);
                return;
            }
            
            if (interactable is IDropZoneInteractable dropZoneInteractable
                && grabInteractor.Holding)
            {
                OnDropZoneInteractableHit(dropZoneInteractable, hitPoint);
                return;
            }
            
            if (interactable is IActivateInteractable activateInteractable
                && !grabInteractor.Holding)
            {
                OnActivateInteractableHit(activateInteractable);
                return;
            }
        }

        private void OnGrabInteractableHit(IGrabInteractable grabInteractable)
        {
            if(grabInteractor.Holding)
                return;
            
            grabInteractor.ProcessInteraction(grabInteractable);
            ui.HoverText = grabInteractable.InteractionTooltip;
        }
        
        private void OnDropZoneInteractableHit(IDropZoneInteractable dropZoneInteractable, Vector3 hitPoint)
        {
            grabInteractor.ProcessInteraction(dropZoneInteractable,hitPoint);
            if(dropZoneInteractable.CanAccept(grabInteractor.HoldingObject))
                ui.HoverText = dropZoneInteractable.InteractionTooltip;
        }
        
        private void OnActivateInteractableHit(IActivateInteractable activateInteractable)
        {
            activateInteractor.ProcessInteraction(activateInteractable);
            ui.HoverText = activateInteractable.InteractionTooltip;
        }

        /// <summary>
        /// Casts a ray and returns interactable which has been hit.
        /// </summary>
        /// <returns>tuple of interactable and hit point</returns>s
        private (IInteractable, Vector3) CastRay()
        {
            var rayTransform = rayOrigin == null ? transform : rayOrigin.transform;
            var rayDirection = rayTransform.forward;
            Debug.DrawRay(transform.position, rayDirection * interactionDistance);

            if (Physics.Raycast(rayTransform.position, rayDirection, out var hit, interactionDistance, rayLayers))
                return (hit.transform.GetComponent<IInteractable>(), hit.point);

            return (null, Vector3.zero);
        }

        private void OnEnable()
        {
            if (ui) ui.Active = true;
        }

        private void OnDisable()
        {
            if (ui) ui.Active = false;
        }
    }
}