namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Handles changes of the quest state
    /// </summary>
    /// <typeparam name="T">type of the state</typeparam>
    public interface IStateChangeHandler<T>
    {
        /// <summary>
        /// Handles state change
        /// </summary>
        /// <param name="args">state change arguments</param>
        void OnStateChanged(StateChangeArgs<T> args);
    }
    
    /// <summary>
    /// Arguments for the state change
    /// </summary>
    /// <typeparam name="T">type of the state</typeparam>
    public struct StateChangeArgs<T>
    {
        public string StateId;
        public T OldState;
        public T NewState;
    }
}