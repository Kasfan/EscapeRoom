using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// State provider that synchronizes the value over the network
    /// </summary>
    /// <typeparam name="TType">type of the state value</typeparam>
    public abstract class NetworkedStateProvider<TType>: NetworkBehaviour, IStateProvider
    {
        private NetworkVariable<TType> value = new NetworkVariable<TType>();

        [SerializeField] 
        [Tooltip("Initial value of the state")]
        public TType startValue;
        
        /// <inheritdoc />
        [field: SerializeField]
        [Tooltip("ID of the target state")]
        public string ID { get; protected set; }

        protected HashSet<IStateChangeHandler<TType>> listeners = new();
        
        /// <inheritdoc />
        public virtual Type StateType => typeof(TType);
        
        public override void OnNetworkSpawn()
        {
            value.OnValueChanged += OnValueChanged;
            
            if (IsServer)
                value.Value = startValue;
        }

        public override void OnNetworkDespawn()
        {
            value.OnValueChanged -= OnValueChanged;
        }


        private void OnValueChanged(TType previousvalue, TType newvalue)
        {
            var changeArgs = new StateChangeArgs<TType>()
            {
                OldState = previousvalue,
                NewState = newvalue,
                StateId = ID
            };
            foreach (var listener in listeners)
                listener.OnStateChanged(changeArgs);
        }

        /// <inheritdoc />
        public T GetStateValue<T>()
        {
            if (typeof(T) != typeof(bool))
                throw new InvalidCastException($"Invalid type, can only return {typeof(bool)} value.");
            
            if(!IsSpawned)
                return (T)(object)startValue;
            
            return (T)(object)value.Value;
        }

        /// <inheritdoc />
        public void SetStateValue<T>(T value)
        {
            if (typeof(T) != typeof(bool))
                throw new InvalidCastException($"Invalid type, can only set {typeof(bool)} value.");

            if (!IsSpawned)
                throw new Exception($"Can't change value until the {nameof(NetworkedStateProvider<TType>)} is spawned");

            if (IsServer)
                this.value.Value = (TType)(object)value;
            else
                SetValueRpc((TType)(object)value);
        }
        
        [Rpc(SendTo.Server)]
        public void SetValueRpc(TType value)
        {
            this.value.Value = value;
        }

        /// <inheritdoc />
        public void AddHandler<T>(IStateChangeHandler<T> handler)
        {
            if (typeof(T) != typeof(TType))
                throw new InvalidCastException($"Invalid type, only {typeof(IStateChangeHandler<TType>)} handler can subscribe.");

            listeners.Add((IStateChangeHandler<TType>)handler);
        }

        /// <inheritdoc />
        public void RemoveHandler<T>(IStateChangeHandler<T> handler)
        {
            if (typeof(T) != typeof(TType))
                throw new InvalidCastException($"Invalid type, only {typeof(IStateChangeHandler<TType>)} handler can unsubscribe.");
            
            listeners.Remove((IStateChangeHandler<TType>)handler);
        }
    }
}