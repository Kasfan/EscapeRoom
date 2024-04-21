using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;
using Object = UnityEngine.Object;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Proxy of a <see cref="IStateProvider"/>, allows easy access to state value and its mutation
    /// </summary>
    /// <typeparam name="T">type of the state value</typeparam>
    public abstract class StateProxyComponent<T> : MonoBehaviour, IStateChangeHandler<T>
    {
        [SerializeReference]
        [RequireInterface(typeof(IStateDatabase))]
        [Tooltip("Optional. If not provided, the global one will be used.")]
        private Object stateDatabase;

        [SerializeField] [Tooltip("Id of the target state to use.")]
        private string targetStateId;

        protected IStateProvider StateProvider;

        /// <summary>
        /// Current value of the state
        /// </summary>
        public T Value
        {
            get => StateProvider.GetStateValue<T>();
            set => StateProvider.SetStateValue(value);
        }

        public virtual void Start()
        {
            var database = stateDatabase ? (IStateDatabase)stateDatabase : EscapeRoomManager.Instance.StateDatabase;

            // subscribe to the new provider and update value
            try
            {
                StateProvider = database.GetStateProvider(targetStateId);
                StateProvider.AddHandler(this);

                OnStateChanged(Value);
            }
            catch (InvalidCastException e)
            {
                Debug.LogError(e);
            }
        }

        /// <inheritdoc/>
        public void OnStateChanged(StateChangeArgs<T> args)
        {
            if (Equals(args.NewState, args.OldState))
                return;

            OnStateChanged(args.NewState);
        }

        /// <summary>
        /// When detected state change.
        /// <remarks>
        /// Mandatory fires one time on start
        /// </remarks>
        /// </summary>
        /// <param name="val">new value</param>
        protected abstract void OnStateChanged(T val);
    }
}