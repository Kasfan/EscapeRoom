using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactable drop zone for <see cref="IGrabInteractable"/> objects
    /// </summary>
    public interface IDropZoneInteractable: IInteractable
    {
        /// <summary>
        /// If drop zone empty or not.
        /// </summary>
        bool Empty { get; }
        
        /// <summary>
        /// Grab interactable dropped in here
        /// </summary>
        IGrabInteractable DroppedInteractable { get; }

        /// <summary>
        /// If provided grab interactable can be dropped here
        /// </summary>
        /// <param name="interactable">interactable to drop</param>
        /// <returns>if the interactable can be drooped</returns>
        bool CanAccept(IGrabInteractable interactable);

        /// <summary>
        /// Tries to attach the interactable to the dropzone
        /// </summary>
        /// <param name="interactable">interactable to drop</param>
        /// <param name="point">drop position of the interactable</param>
        void TryDropInteractable(IGrabInteractable interactable, Vector3 point);

    }
}