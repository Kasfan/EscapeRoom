using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace EscapeRoom.SceneManagement
{
    /// <summary>
    /// Component that detects when a trigger enters or leaves a zone in scene space.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class TriggerZoneEvents: MonoBehaviour
    {
        /// <summary>
        /// Invoked when a trigger enters the collider
        /// </summary>
        [FormerlySerializedAs("OnTriggerEntered")] 
        public UnityEvent onTriggerEntered;
        
        /// <summary>
        /// Invoked when a trigger exits the collider
        /// </summary>
        [FormerlySerializedAs("OnTriggerLeft")] 
        public UnityEvent onTriggerLeft;
        
        /// <summary>
        /// Invoked repeatedly while a trigger is in the collider
        /// </summary>
        public UnityEvent onTriggerStay;

        /// <summary>
        /// If not null, only notifies when the <see cref="TargetObject"/> enters and exits
        /// </summary>
        [field: SerializeField]
        public GameObject TargetObject { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (TargetObject == null)
            {
                onTriggerEntered?.Invoke();
                return;
            }
            
            if (other.gameObject == TargetObject)
                onTriggerEntered?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {            
            if (TargetObject == null)
            {
                onTriggerLeft?.Invoke();
                return;
            }

            if(other.gameObject == TargetObject)
                onTriggerLeft?.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (TargetObject == null)
            {
                onTriggerStay?.Invoke();
                return;
            }

            if(other.gameObject == TargetObject)
                onTriggerStay?.Invoke();
        }
    }
}