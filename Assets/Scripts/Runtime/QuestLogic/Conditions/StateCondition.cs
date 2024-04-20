using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;
using Object = UnityEngine.Object;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// A condition that is true when target state satisfies expected value
    /// <remarks>
    /// Unity does not support generic type serialization, for <see cref="MonoBehaviour"/> use concrete types instead.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">type of the state value</typeparam>
    [Serializable]
    public class StateCondition<T>: Condition, IStateChangeHandler<T>, IInitializable, IDisposable
    {
        [SerializeReference] 
        [RequireInterface(typeof(IStateDatabase))]
        [Tooltip("Optional. If not provided, the global one will be used.")]
        private Object stateDatabase;
        
        [SerializeField] 
        [Tooltip("Id of the target state to use.")]
        private string targetStateId;
        
        [SerializeField] 
        [Tooltip("Target value of the condition.")]
        private T targetValue;

        private T currentValue;

        private IStateProvider stateProvider;

        /// <inheritdoc/>
        public bool Initialized { get; private set; }

        /// <inheritdoc/>
        public override bool IsTrue
        {
            get
            {
                var state = stateProvider != null && targetValue.Equals(currentValue);
                return invert ? !state : state;
            }
        }
        
        /// <inheritdoc/>
        public void OnStateChanged(StateChangeArgs<T> args)
        {
            currentValue = args.NewState;
        }

        /// <summary>
        /// Assigns a <see cref="IStateProvider"/> to the condition
        /// </summary>
        /// <param name="provider">target provider</param>
        public void SetStateProvider(IStateProvider provider)
        {
            // unsubscribe from previous provider if exists
            if (stateProvider != null)
            {
                try
                {

                    stateProvider.RemoveHandler(this);
                    stateProvider = null;

                }
                catch (InvalidCastException e)
                {
                    Debug.LogError(e);
                }
            }
            
            // subscribe to the new provider and update value
            try
            {
                stateProvider.AddHandler(this);
                currentValue = stateProvider.GetStateValue<T>();                
                stateProvider = provider;
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e);
            }
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
            stateProvider?.RemoveHandler(this);
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            var database = stateDatabase? (IStateDatabase)stateDatabase: EscapeRoomManager.Instance.StateDatabase;

            // subscribe to the new provider and update value
            try
            {
                stateProvider = database.GetStateProvider(targetStateId);
                stateProvider.AddHandler(this);
                currentValue = stateProvider.GetStateValue<T>();
                Initialized = true;
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e);
            }
        }
    }

    /// <summary>
    /// Int state condition.
    /// </summary>
    /// <inheritdoc cref="StateCondition{T}"/>
    public class StateConditionInt : StateCondition<int>
    {
        
    }
    /// <summary>
    /// Bool state condition.
    /// </summary>
    /// <inheritdoc cref="StateCondition{T}"/>
    public class StateConditionBool : StateCondition<bool>
    {
        
    }
}