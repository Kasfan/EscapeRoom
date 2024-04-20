namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Conditional state true or false
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// If the condition is true or not
        /// </summary>
        bool IsTrue { get; }
    }
}