using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Groups multiple conditions into one
    /// </summary>
    [Serializable]
    public class GroupCondition: Condition, IInitializable, IDisposable
    {
        [SerializeField]
        [Tooltip("If true - all conditions must be true to turn the group true, otherwise any will do")]
        private bool all;
        
        [SerializeReference]
        [Tooltip("Nested conditions")]
        private  List<ICondition> conditions = new();

        /// <summary>
        /// Cached state of the condition. To get accurate condition state call <see cref="IsTrue"/> property.
        /// </summary>
        public bool isTrue { get; private set; }

        /// <inheritdoc/>
        public override bool IsTrue
        {
            get
            {
                isTrue = all ? 
                    AllConditionsTrue(conditions) : 
                    AnyConditionTrue(conditions);

                return invert ? !isTrue : isTrue;
            }
        }

        /// <inheritdoc/>
        public bool Initialized { get; private set; }
        
        /// <summary>
        /// Default CTOR
        /// </summary>
        public GroupCondition(){}

        /// <summary>
        /// CTOR with parameters. Better use for unit tests
        /// </summary>
        /// <param name="all">If true - all conditions must be true to turn the group true, otherwise any will do</param>
        /// <param name="conditions">set of the conditions in the group</param>
        public GroupCondition(bool all, List<ICondition> conditions)
        {
            this.all = all;
            this.conditions = conditions;
        }

        /// <inheritdoc/>
        public void Initialize()
        {
            foreach (var condition in conditions)
            {
                if (condition is IInitializable initializable)
                    initializable.Initialize();
            }

            Initialized = true;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var condition in conditions)
            {
                if(condition is IDisposable disposable)
                    disposable.Dispose();
            }
        }

        /// <summary>
        /// Checks if all the conditions in the group are true
        /// </summary>
        /// <param name="conditions">group of conditions</param>
        /// <returns>all true</returns>
        public static bool AllConditionsTrue(IEnumerable<ICondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if(condition.IsTrue)
                    continue;
                        
                // if this condition is not true, then 'all' condition is not satisfied
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Checks if any condition in the group is true
        /// </summary>
        /// <param name="conditions">group of conditions</param>
        /// <returns>any true</returns>
        public static bool AnyConditionTrue(IEnumerable<ICondition> conditions)
        {
            foreach (var condition in conditions)
            {
                if(condition.IsTrue)
                    continue;
                        
                // if this condition is true, then 'any' condition is satisfied
                return true;
            }

            return false;
        }



        #region Editor

        /// <summary>
        /// Adds a new condition of a provided type.
        /// </summary>
        /// <param name="type"><see cref="ICondition"/></param>
        /// <exception cref="ArgumentException">if provided type does not implement <see cref="ICondition"/> inteface</exception>
        [Button]
        public void AddCondition(Type type)
        {
            if (!typeof(ICondition).IsAssignableFrom(type))
                throw new ArgumentException($"The provided type must implement {nameof(ICondition)} interface");
            
            var newCondition = Activator.CreateInstance(type);
            conditions.Add((ICondition)newCondition);
        }
        
        #endregion
    }
}