using UnityEngine;

namespace Runtime.QuestObjects
{
    /// <summary>
    /// Script that switches door animation states
    /// </summary>
    public class AnimatedFuseBoxLever: MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("Animator of the door that animates on-of states")]
        private Animator animator;

        [SerializeField] 
        [Tooltip("Name of the bool parameter for on state.")]
        private string isOnPropertyName = "IsOn";

        
        /// <summary>s
        /// Whether the power is On or Off
        /// </summary>
        public bool IsOn
        {
            get => animator.GetBool(isOnPropertyName);
            set => animator.SetBool(isOnPropertyName, value);
        }
    }
}