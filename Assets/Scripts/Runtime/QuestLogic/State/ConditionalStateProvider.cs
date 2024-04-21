using System;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// State provider that changed by a condition
    /// </summary>
    public class ConditionalStateProvider: MonoBehaviour, IStateProvider
    {
        /// <inheritdoc />
        [field: SerializeField]
        [Tooltip("ID of the target state")]
        public string ID { get; protected set; }
        
        [SerializeReference] 
        protected GroupCondition condition = new();
        
        private bool lastConditionState;

        private HashSet<IStateChangeHandler<bool>> listeners = new();

        private void Start()
        {
            condition.Initialize();
        }

        private void Update()
        {
            var newState = condition.IsTrue;
            if (lastConditionState == newState)
                return;

            var changeArgs = new StateChangeArgs<bool>()
            {
                NewState = newState,
                OldState = lastConditionState,
                StateId = ID
            };
            foreach (var listener in listeners)
                listener.OnStateChanged(changeArgs);

            lastConditionState = newState;
        }

        /// <inheritdoc />
        public Type StateType => typeof(bool);
        
        /// <inheritdoc />
        public T GetStateValue<T>()
        {
            if (typeof(T) != typeof(bool))
                throw new InvalidCastException($"Invalid type, {nameof(ConditionalStateProvider)} can only return {typeof(bool)} value.");
            
            return (T)(object)condition.isTrue;
        }

        /// <inheritdoc />
        public void SetStateValue<T>(T value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void AddHandler<T>(IStateChangeHandler<T> handler)
        {
            if (typeof(T) != typeof(bool))
                throw new InvalidCastException($"Invalid type, only {typeof(IStateChangeHandler<bool>)} handler can subscribe.");

            listeners.Add((IStateChangeHandler<bool>)handler);
        }

        /// <inheritdoc />
        public void RemoveHandler<T>(IStateChangeHandler<T> handler)
        {
            if (typeof(T) != typeof(bool))
                throw new InvalidCastException($"Invalid type, only {typeof(IStateChangeHandler<bool>)} handler can unsubscribe.");
            
            listeners.Remove((IStateChangeHandler<bool>)handler);
        }
    }
}