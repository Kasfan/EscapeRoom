using Runtime.Networking;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Internal;

namespace EscapeRoom.Player
{
    /// <summary>
    /// Spawn point components storage
    /// </summary>
    public class ServerPlayerSpawnPoints : MonoBehaviour, ISpawnPointProvider
    {
        [SerializeField] 
        [RequireInterface(typeof(ISpawnPoint))]
        Object[] spawnPoints;

        /// <inheritdoc/>
        public ISpawnPoint GetRandomPoint()
        {
            if (spawnPoints.Length == 0)
                return null;

            return (ISpawnPoint)spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
    }
}
