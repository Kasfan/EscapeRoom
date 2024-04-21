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

        private void Update()
        {
            var (interactable, hitPoint) = CastRay();

            if (interactable is IGrabInteractable grabInteractable)
            {
                OnGrabInteractableHit(grabInteractable);
                return;
            }
            
            if (interactable is IDropZoneInteractable dropZoneInteractable)
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

            if (ui) ui.HoverText = "";
        }

        private void OnGrabInteractableHit(IGrabInteractable grabInteractable)
        {
            if(grabInteractor.Holding)
                return;
            
            grabInteractor.ProcessInteraction(grabInteractable);
            if(ui) ui.HoverText = grabInteractable.InteractionTooltip;
        }
        
        private void OnDropZoneInteractableHit(IDropZoneInteractable dropZoneInteractable, Vector3 hitPoint)
        {
            if (grabInteractor.Holding)
            {
                grabInteractor.ProcessInteraction(dropZoneInteractable,hitPoint);
                if(ui && dropZoneInteractable.CanAccept(grabInteractor.HoldingObject))
                    ui.HoverText = dropZoneInteractable.InteractionTooltip;
                return;
            }
            
            // if grab interactor can grab and drop zone contains object, treat it like ray hit the object
            var grabInteractable = dropZoneInteractable.DroppedInteractable;
            if (grabInteractable != null )
            {
                OnGrabInteractableHit(grabInteractable);
            }

        }
        
        private void OnActivateInteractableHit(IActivateInteractable activateInteractable)
        {
            activateInteractor.ProcessInteraction(activateInteractable);
            if(ui) ui.HoverText = activateInteractable.InteractionTooltip;
        }

        /// <summary>
        /// Casts a ray and returns interactable which has been hit.
        /// </summary>
        /// <returns>tuple of interactable and hit point</returns>s
        private (IInteractable, Vector3) CastRay()
        {
            var rayTransform = rayOrigin == null ? transform : rayOrigin.transform;
            var rayDirection = rayTransform.forward;
            Debug.DrawRay(rayTransform.position, rayDirection * interactionDistance);

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