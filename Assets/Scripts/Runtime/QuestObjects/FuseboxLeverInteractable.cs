using EscapeRoom.QuestLogic;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Activate interactable for FuseboxLever
    /// </summary>
    [RequireComponent(typeof(BoolStateProxyComponent))]
    public class FuseboxLeverInteractable: MonoBehaviour, IActivateInteractable
    {
        /// <inheritdoc/>
        public string InteractionTooltip
        {
            get
            {
                if (stateProxyComponent == null)
                    return "Switch";

                return stateProxyComponent.Value ? "Turn Off" : "Turn On";
            }
        }

        private BoolStateProxyComponent stateProxyComponent;

        private void Awake()
        {
            stateProxyComponent = GetComponent<BoolStateProxyComponent>();
        }

        /// <inheritdoc/>
        public void OnActivated()
        {
            stateProxyComponent.Value = !stateProxyComponent.Value;
        }
    }
}