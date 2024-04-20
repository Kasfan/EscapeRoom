using UnityEngine;

namespace Runtime.Networking
{
    public interface ISpawnPoint
    {
        /// <summary>
        /// If the spawned object should be rotated in the same direction as this point
        /// </summary>
        bool MatchRotation { get; }
        
        /// <summary>
        /// Global position of the spawnpoint
        /// </summary>
        Vector3 Position { get; }
        
        /// <summary>
        /// Global rotation of the spawnpoint
        /// </summary>
        Quaternion Rotation { get; }
    }
}