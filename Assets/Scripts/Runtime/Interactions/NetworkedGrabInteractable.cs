using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Simple networked grab interactable
    /// </summary>
    public class NetworkedGrabInteractable: NetworkBehaviour, IGrabInteractable
    {
        /// <inheritdoc/>
        [field: SerializeField]
        [Tooltip("Hint message that will be shown, when player hovers object")]
        public string InteractionTooltip { get; protected set; }

        /// <inheritdoc/>
        public Transform Transform => transform;

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