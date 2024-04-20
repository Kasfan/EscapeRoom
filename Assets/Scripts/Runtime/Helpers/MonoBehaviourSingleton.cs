using UnityEngine;

namespace EscapeRoom.Helpers
{
    /// <summary>
    /// Singleton MonoBehaviour that gets found in the scene or created if does not exist
    /// </summary>
    /// <typeparam name="T">type of the component</typeparam>
    public class MonoBehaviourSingleton<T> : MonoBehaviour
        where T : Component
    {
        private static T _instance;

        /// <summary>
        /// Current instance of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                        _instance = objs[0];
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// Singleton MonoBehaviour that should be added to the scene manually
    /// </summary>
    /// <typeparam name="T">type of the component</typeparam>
    public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        /// <summary>
        /// Current instance of the singleton
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Unity lifetime
        /// </summary>
        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}