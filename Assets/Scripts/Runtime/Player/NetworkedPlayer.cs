using EscapeRoom.Interactions;
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
        CapsuleCollider m_CapsuleCollider;

        private FirstPersonController fpvController;
        private CharacterController characterController;
        private PlayerInput playerInput;
        private PlayerInputManager playerInputManager;
        private RayInteractor rayInteractor;
        
        /// <inheritdoc/>
        public Transform CameraAttach => fpvController.CameraView;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            fpvController = GetComponent<FirstPersonController>();
            characterController = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();
            playerInputManager = GetComponent<PlayerInputManager>();
            rayInteractor = GetComponent<RayInteractor>();

            // disable everything until the player spawns
            if (m_CapsuleCollider) m_CapsuleCollider.enabled = false;
            if (fpvController) fpvController.enabled = false;
            if (characterController)  characterController.enabled = false;
            if (playerInput) playerInput.enabled = false;
            if (playerInputManager) playerInputManager.enabled = false;
            if (rayInteractor) rayInteractor.enabled = false;
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

            var point = EscapeRoomManager.Instance.SpawnPointProvider.GetRandomPoint();
            if (point != null)
            {
                transform.position = point.Position;
                transform.rotation = point.MatchRotation ? point.Rotation : transform.rotation;
            }

            // enable controls only on owning client
            if (m_CapsuleCollider) m_CapsuleCollider.enabled = true;
            if (fpvController) fpvController.enabled = true;
            if (characterController)  characterController.enabled = true;
            if (playerInput) playerInput.enabled = true;
            if (playerInputManager) playerInputManager.enabled = true;
            if (rayInteractor) rayInteractor.enabled = true;

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