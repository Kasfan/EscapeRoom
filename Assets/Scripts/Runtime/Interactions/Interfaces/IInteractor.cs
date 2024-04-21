namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Component that can interact with interactable
    /// </summary>
    /// <typeparam name="T">target interactor type</typeparam>
    public interface IInteractor<T> where T: IInteractable
    {
        /// <summary>
        /// Processes interaction with provided interactable
        /// </summary>
        /// <param name="interactable">interactor to process</param>
        void ProcessInteraction(T interactable);
    }
}