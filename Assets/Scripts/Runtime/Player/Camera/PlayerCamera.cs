using Runtime.Networking;
using UnityEngine;

namespace EscapeRoom.Player
{
    /// <summary>
    /// Camera controlled by a player
    /// </summary>
    public class PlayerCamera: MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Component that moves camera when it's not attached to the player")]
        private MonoBehaviour freeLookCamera;

        /// <summary>
        /// Attaches camera to the player
        /// </summary>
        /// <param name="player">target player</param>
        public void AttachToPlayer(IPlayer player)
        {
            if (player == null)
            {
                transform.parent = null;
                
                if (freeLookCamera)
                    freeLookCamera.enabled = true;
                return;
            }

            if (freeLookCamera)
                freeLookCamera.enabled = false;
            
            transform.parent = player.CameraAttach;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
        }
    }
}