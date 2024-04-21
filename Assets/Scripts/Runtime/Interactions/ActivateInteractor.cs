using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactor that activates an object when input action is triggered
    /// </summary>
    public class ActivateInteractor: MonoBehaviour, IInteractor<IActivateInteractable>
    {
        [SerializeField]
        private InputActionReference activateAction;
        
        /// <inheritdoc/>
        public void ProcessInteraction(IActivateInteractable interactable)
        {
            if (activateAction.action.triggered)
                interactable.OnActivated();
        }
    }
}