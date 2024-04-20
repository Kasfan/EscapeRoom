using System;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Provides a state
    /// </summary>
    public interface IStateProvider
    {
        /// <summary>
        /// ID of the state
        /// </summary>
        string ID { get; }
        
        /// <summary>
        /// Type of the state
        /// </summary>
        Type StateType { get; }
        
        /// <summary>
        /// Returns value of the state
        /// </summary>
        /// <typeparam name="T">type of the state (eg, Int, Bool)</typeparam>
        /// <returns>current value of the state</returns>
        /// <exception cref="InvalidCastException">thrown if current state type is not T</exception> 
        T GetStateValue<T>();

        /// <summary>
        /// Sets the state value
        /// </summary>
        /// <param name="value">new value</param>
        /// <typeparam name="T">type of the state (eg, Int, Bool)</typeparam>
        /// <exception cref="InvalidCastException">thrown if current state type is not T</exception> 
        void SetStateValue<T>(T value);

        /// <summary>
        /// Adds handler of the state change
        /// </summary>
        /// <param name="handler">handler to subscribe</param>
        /// <typeparam name="T">Type of the state</typeparam>
        /// <exception cref="InvalidCastException">thrown if current state type is not T</exception> 
        void AddHandler<T>(IStateChangeHandler<T> handler);
        
        /// <summary>
        /// Removes handler of the state change
        /// </summary>
        /// <param name="handler">handler to unsubscribe</param>
        /// <typeparam name="T">Type of the state</typeparam>
        /// <exception cref="InvalidCastException">thrown if current state type is not T</exception> 
        void RemoveHandler<T>(IStateChangeHandler<T> handler);
    }
}