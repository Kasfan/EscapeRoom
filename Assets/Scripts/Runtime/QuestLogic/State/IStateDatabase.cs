using System;
using System.Collections.Generic;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Stores quest states 
    /// </summary>
    public interface IStateDatabase
    {
        /// <summary>
        /// All stored states
        /// </summary>
        IEnumerable<IStateProvider> States { get; }
        
        /// <summary>
        /// Returns a provider of the quest state if exists
        /// </summary>
        /// <param name="id">id of the state</param>
        /// <returns>state provider if exists, Null otherwise</returns>
        IStateProvider GetStateProvider(string id);

        /// <summary>
        /// Returns a quest state provider of particular type if exists
        /// </summary>
        /// <param name="id">id of the state</param>
        /// <param name="stateType">type of the state value</param>
        /// <returns>state provider if exists, Null otherwise</returns>
        IStateProvider GetStateProvider(string id, Type stateType);
    }
}