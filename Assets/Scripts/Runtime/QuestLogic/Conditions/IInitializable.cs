namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Defines a component that should be initialized
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// If the object is initialized
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Initialize the object
        /// </summary>
        void Initialize();
    }
}