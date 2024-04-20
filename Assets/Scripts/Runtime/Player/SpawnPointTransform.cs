using Runtime.Networking;
using UnityEngine;

namespace EscapeRoom.Player
{
    /// <summary>
    /// Simple spawnpoint transform
    /// </summary>
    public class SpawnPointTransform: MonoBehaviour, ISpawnPoint
    {
        /// <inheritdoc/>
        [field: SerializeField]
        public bool MatchRotation { get; set; }

        /// <inheritdoc/>
        public Vector3 Position => transform.position;

        /// <inheritdoc/>
        public Quaternion Rotation => transform.rotation;
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "BodySilhouette", true);
        }
    }
}