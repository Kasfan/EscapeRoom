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

        //void OnPickUp()
        //{
        //    if (m_ServerPlayerMove.isObjectPickedUp.Value)
        //    {
        //        m_ServerPlayerMove.DropObjectServerRpc();
        //    }
        //    else
        //    {
        //        // detect nearby ingredients
        //        var hits = Physics.BoxCastNonAlloc(transform.position,
        //            Vector3.one,
        //            transform.forward,
        //            m_HitColliders,
        //            Quaternion.identity,
        //            1f,
        //            LayerMask.GetMask(new[] { "PickupItems" }),
        //            QueryTriggerInteraction.Ignore);
        //        if (hits > 0)
        //        {
        //            var ingredient = m_HitColliders[0].collider.gameObject.GetComponent<ServerIngredient>();
        //            if (ingredient != null)
        //            {
        //                var netObj = ingredient.NetworkObjectId;
        //                // Netcode is a server driven SDK. Shared objects like ingredients need to be interacted with using ServerRPCs. Therefore, there
        //                // will be a delay between the button press and the reparenting.
        //                // This delay could be hidden with some animations/sounds/VFX that would be triggered here.
        //                m_ServerPlayerMove.PickupObjectServerRpc(netObj);
        //            }
        //        }
        //    }
        //}
    }
}