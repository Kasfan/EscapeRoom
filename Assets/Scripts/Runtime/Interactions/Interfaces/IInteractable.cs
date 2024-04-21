namespace EscapeRoom.Interactions
{
    /// <summary>
    /// Base interface for an interactable object
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Hint message about available interaction
        /// </summary>
        string InteractionTooltip { get; }
    }
}