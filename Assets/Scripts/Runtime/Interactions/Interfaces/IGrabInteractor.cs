using UnityEngine;

namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactor that can grab and drop <see cref="IGrabInteractable"/> objects
    /// </summary>
    public interface IGrabInteractor: IInteractor<IGrabInteractable>
    {
        /// <summary>
        /// Whether or not interactor is already holding an object
        /// </summary>
        bool Holding { get; }
        
        /// <summary>
        /// Object that the interactor currently holding
        /// </summary>
        IGrabInteractable HoldingObject { get; }
        
        /// <summary>
        /// Point to attach grabbable
        /// </summary>
        public Transform attachPoint { get; }

        /// <summary>
        /// Processes interaction with provided interactable
        /// </summary>
        /// <param name="dropZone">target dropzone</param>
        /// <param name="interactionPoint">point in space where interaction happened</param>
        void ProcessInteraction(IDropZoneInteractable dropZone, Vector3 interactionPoint);
    }
}