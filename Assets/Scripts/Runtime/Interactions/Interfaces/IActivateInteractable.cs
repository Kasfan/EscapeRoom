namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Interactable that can be activated
    /// </summary>
    public interface IActivateInteractable: IInteractable
    {
        /// <summary>
        /// Called when the interactable is activated
        /// </summary>
        void OnActivated();
    }
}