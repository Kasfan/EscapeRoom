using EscapeRoom.Helpers;
using EscapeRoom.Player;
using Runtime.Networking;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;

namespace EscapeRoom.QuestLogic
{
    /// <summary>
    /// Main manager of the escape room game
    /// </summary>
    public class EscapeRoomManager: MonoBehaviourSingletonPersistent<EscapeRoomManager>
    {
        [SerializeField]
        [RequireInterface(typeof(IStateDatabase))]
        [Tooltip("Global state database of the escape room")]
        private Object stateDatabase;

        [SerializeField]
        [RequireInterface(typeof(ISpawnPointProvider))]
        [Tooltip("Quest spawn points provider")]
        private Object spawnPointProvider;

        [field: SerializeField] 
        [Tooltip("Camera that attaches to local player")]
        public PlayerCamera PlayerCamera { get; set; }
        
        /// <summary>
        /// Database of the escape room states
        /// </summary>
        public IStateDatabase StateDatabase => (IStateDatabase)stateDatabase;

        /// <summary>
        /// Quest spawn points provider
        /// </summary>
        public ISpawnPointProvider SpawnPointProvider => (ISpawnPointProvider)spawnPointProvider;

    }
}