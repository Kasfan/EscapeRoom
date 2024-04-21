namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactable that can be grabbed and dropped
    /// </summary>
    public interface IGrabInteractable: IInteractable
    {
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