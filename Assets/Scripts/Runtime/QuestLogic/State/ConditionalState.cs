using System;
using UnityEngine;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// State provider that changed by a condition
    /// </summary>
    public class ConditionalStateProvider: MonoBehaviour, IStateProvider
    {
        [SerializeField]
        [Tooltip("ID of the target state")]
        protected string stateId;

        [SerializeReference] 
        protected GroupCondition condition = new GroupCondition();
        
        /// <summary>
        /// Invoked when state of the condition changes
        /// </summary>
        public event Action ConditionChanged;
        
        public string ID { get; }
        public Type StateType { get; }
        public T GetStateValue<T>()
        {
            throw new NotImplementedException();
        }

        public void SetStateValue<T>(T value)
        {
            throw new NotImplementedException();
        }

        public void AddHandler<T>(IStateChangeHandler<T> handler)
        {
            throw new NotImplementedException();
        }

        public void RemoveHandler<T>(IStateChangeHandler<T> handler)
        {
            throw new NotImplementedException();
        }
    }
}