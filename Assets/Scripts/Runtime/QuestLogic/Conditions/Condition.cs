using System;
using UnityEngine;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Base class for a condition
    /// </summary>
    [Serializable]
    public abstract class Condition: ICondition
    {
        [SerializeField]
        protected string name;
        
        [SerializeField]
        [Tooltip("Invert the condition output")]
        protected bool invert;

        /// <summary>
        /// If the condition is true
        /// </summary>
        public abstract bool IsTrue { get; }
    }
}