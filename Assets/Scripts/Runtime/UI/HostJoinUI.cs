using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeRoom.UI
{
    public class HostJoinUI : MonoBehaviour
    {
        [SerializeField] 
        private bool overrideEndpoint;
        
        [SerializeField] 
        private TMP_InputField ipTextField;

        [SerializeField] 
        private TMP_InputField portTextField;

        [SerializeField] 
        private GameObject menu;
        
        [SerializeField] 
        private Button startHostBtn;
        
        [SerializeField] 
        private Button startServerBtn;
        
        [SerializeField] 
        private Button startClientBtn;

        void Awake()
        {
            startHostBtn.onClick.AddListener(StartHost);
            startServerBtn.onClick.AddListener(StartServer);
            startClientBtn.onClick.AddListener(StartClient);
        }

        void OnDestroy()
        {
            startHostBtn.onClick.RemoveListener(StartHost);
            startServerBtn.onClick.RemoveListener(StartServer);
            startClientBtn.onClick.RemoveListener(StartClient);
        }

        void Start()
        {
            ToggleMainMenuUI(true);
            if (!overrideEndpoint)
            {
                var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
                ipTextField.text = utp.ConnectionData.Address;
                portTextField.text = utp.ConnectionData.Port.ToString();
                ipTextField.interactable = false;
                portTextField.interactable = false;
            }
        }

        void StartHost()
        {
            ipTextField.text = "0.0.0.0";
            if(overrideEndpoint)
                SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartHost();
            if (result)
            {
                ToggleMainMenuUI(false);
            }
        }

        void StartClient()
        {
            if(overrideEndpoint)
                SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartClient();
            if (result)
            {
                ToggleMainMenuUI(false);
            }
        }

        void StartServer()
        {
            ipTextField.text = "0.0.0.0";
            if(overrideEndpoint)
                SetUtpConnectionData();
            var result = NetworkManager.Singleton.StartServer();
            if (result)
            {
                ToggleMainMenuUI(false);
            }
        }

        void ToggleMainMenuUI(bool isVisible)
        {
            menu.SetActive(isVisible);
        }

        void SetUtpConnectionData()
        {
            var sanitizedIPText = Sanitize(ipTextField.text);
            var sanitizedPortText = Sanitize(portTextField.text);

            Debug.Log($"sanitizedIPText: {sanitizedIPText}, sanitizedPortText: {sanitizedPortText}");
            ushort.TryParse(sanitizedPortText, out var port);

            var utp = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            utp.SetConnectionData(sanitizedIPText, port);
        }

        /// <summary>
        /// Sanitize user port InputField box allowing only alphanumerics and '.'
        /// </summary>
        /// <param name="dirtyString"> string to sanitize. </param>
        /// <returns> Sanitized text string. </returns>
        static string Sanitize(string dirtyString)
        {
            return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
        }
    }
}