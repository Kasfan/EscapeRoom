using EscapeRoom.Player.Controllers;
using EscapeRoom.QuestLogic;
using Runtime.Networking;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoom.Player
{
    /// <summary>
    /// <remarks>
    /// Assumes client authority
    /// </remarks>
    /// </summary>
    [DefaultExecutionOrder(1)] // after server component
    public class NetworkedPlayer : NetworkBehaviour, IPlayer
    {
        [SerializeField] 
        private FirstPersonController fpvController;

        [SerializeField] 
        private CharacterController characterController;

        [SerializeField] 
        private PlayerInput playerInput;

        [SerializeField] 
        CapsuleCollider m_CapsuleCollider;

        /// <inheritdoc/>
        public Transform CameraAttach => fpvController.CameraView;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // disable everything until the player spawns
            fpvController.enabled = false;
            characterController.enabled = false;
            m_CapsuleCollider.enabled = false;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            enabled = IsClient;
            if (!IsOwner)
            {
                enabled = false;
                m_CapsuleCollider.enabled = true;
                return;
            }

            // enable controls only on owning client
            playerInput.enabled = true;
            fpvController.enabled = true;
            characterController.enabled = true;

            // attach camera to the prefab
            EscapeRoomManager.Instance.PlayerCamera.AttachToPlayer(this);
        }
        
        public override void OnNetworkDespawn()
        {
            // free the camera
            EscapeRoomManager.Instance.PlayerCamera.AttachToPlayer(null);
        }
    }
}