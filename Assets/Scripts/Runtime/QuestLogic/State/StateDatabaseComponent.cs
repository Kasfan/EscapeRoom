using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;
using Object = UnityEngine.Object;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Simple state database that stores states providers as list of references to components
    /// </summary>
    public class StateDatabaseComponent: MonoBehaviour, IStateDatabase
    {
        [SerializeField] 
        [RequireInterface(typeof(IStateProvider))]
        [Tooltip("State provider components")]
        private List<Object> stateProviders;

        /// <inheritdoc/>
        public IEnumerable<IStateProvider> States
        {
            get
            {
                return stateProviders.OfType<IStateProvider>().ToList();
            }
        }

        /// <inheritdoc/>
        public IStateProvider GetStateProvider(string id)
        {
            return States.FirstOrDefault(state => state.ID == id);
        }

        /// <inheritdoc/>
        public IStateProvider GetStateProvider(string id, Type stateType)
        {
            return States.FirstOrDefault(state => state.ID == id && state.StateType == stateType);
        }

        /// <summary>
        /// Finds and adds all components that implement 
        /// </summary>
        public void FindChildrenStateProviders()
        {
            stateProviders = GetComponentsInChildren<IStateProvider>().OfType<Object>().ToList();
        }

        /// <summary>
        /// Unity Editor lifetime.
        /// Validate that all the state providers have unique IDs
        /// </summary>
        public void OnValidate()
        {
            var duplicateIds = States
                .GroupBy(state => state.ID) 
                .Where(stateGroup => stateGroup.Count() > 1) 
                .Select(stateGroup => stateGroup.Key) 
                .ToList();

            if (duplicateIds.Count == 0)
                return;

            foreach (var id in duplicateIds)
            {
                Debug.LogError($"State database can not contain states with the same ID. Duplicate: {id}");
            }
        }
    }
}