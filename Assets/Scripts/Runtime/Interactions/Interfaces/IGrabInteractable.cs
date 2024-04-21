using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactable that can be grabbed and dropped
    /// </summary>
    public interface IGrabInteractable: IInteractable
    {
        /// <summary>
        /// Transform of the grab interactable
        /// </summary>
        public Transform Transform { get; }
        
        /// <summary>
        /// Called when the object is grabbed
        /// </summary>
        void OnGrabbed();
        
        /// <summary>
        /// Called when the object is dropped
        /// </summary>
        void OnDropped();
    }
}