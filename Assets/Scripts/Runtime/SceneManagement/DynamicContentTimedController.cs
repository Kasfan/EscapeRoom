using System;
using UnityEngine;

namespace EscapeRoom.SceneManagement
{
    /// <summary>
    /// Controls when the dynamic content should be loaded and unloaded.
    /// When gets activated - loads the content. Then counts <see cref="deactivationDelay"/> and unloads the content.
    /// The activated timer can be reset by calling <see cref="Activate"/> again.
    /// </summary>
    [RequireComponent(typeof(DynamicContentLoader))]
    public class DynamicContentTimedController: MonoBehaviour
    {
        [SerializeField] 
        [Tooltip("Wait time before automatically deactivating")]
        private float deactivationDelay = 5f;

        private float deactivationTimer = -1f;

        private DynamicContentLoader dynamicContentLoader;
        
        private bool activated;

        public bool Activated
        {
            get => activated;
            private set
            {
                if (activated == value) return;
                
                activated = value;
                if(activated)
                    dynamicContentLoader.Load();
                else
                    dynamicContentLoader.Unload();
            }
        }

        private void Awake()
        {
            dynamicContentLoader = GetComponent<DynamicContentLoader>();
        }
        
        public void Activate()
        {
            Activated = true;
            deactivationTimer = deactivationDelay;
        }

        private void Update()
        {
            if(!Activated)
                return;
            
            deactivationTimer -= Time.deltaTime;
            if (deactivationTimer < 0f)
                Activated = false;
        }
    }
}