using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    public class NetworkedGrabbable: NetworkBehaviour, IGrabInteractable
    {
        /// <inheritdoc/>
        [field: SerializeField]
        [Tooltip("Hint message that will be shown, when player hovers object")]
        public string InteractionTooltip { get; protected set; }

        /// <inheritdoc/>
        public void OnGrabbed()
        {
        }
        
        /// <inheritdoc/>
        public void OnDropped()
        {

        }
    }
}