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
        private InputAction activateAction;
        
        /// <inheritdoc/>
        public void ProcessInteraction(IActivateInteractable interactable)
        {
            if (activateAction.triggered)
                interactable.OnActivated();
        }
    }
}