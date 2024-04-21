using UnityEngine;

namespace EscapeRoom.QuestObjects
{
    /// <summary>
    /// Script that switches door animation states
    /// </summary>
    public class AnimatedDoor: MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("The door open state on start")]
        private bool openOnStart;
        
        [SerializeField] 
        [Tooltip("Animator of the door that animates open state")]
        private Animator animator;

        [SerializeField] 
        [Tooltip("Name of the bool parameter for open state.")]
        private string openPropertyName = "Open";
        
        /// <summary>
        /// Whether the door is open or not
        /// </summary>
        public bool Open
        {
            get => animator.GetBool(openPropertyName);
            set => animator.SetBool(openPropertyName, value);
        }

        private void Awake()
        {
            Open = openOnStart;
        }
    }
}