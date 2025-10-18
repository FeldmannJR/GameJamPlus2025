using System;
using System.Net;
using Cysharp.Threading.Tasks;
using FishNet.Managing;
using FishNet.Transporting;
using FishNet.Transporting.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ConnectUI : GameUI
    {
        [SerializeField] private NetworkManager _networkManager;
        [Q] private Button _connect;
        [Q] private Button _startServer;
        [Q] private TextField _codeField;
        [Q] private Label _yourCode;
        [Q] private Label _message;
        [Q] private VisualElement _connectContainer;
        [Q] private Button _quitButton;

        private string _code;
        private bool _connecting = false;
        private bool _connected = false;


        public override void OnOpen(VisualElement root)
        {
            _startServer.clicked += StartServer;
            _connect.clicked += OnClick_Client;
            _yourCode.style.display = DisplayStyle.None;
            _quitButton.clicked += () => Application.Quit();
            _message.text = "";
        }


        public void OnClick_Server()
        {
            RequestServer().Forget();
            /*if (_networkManager == null)
                return;

            if (_serverState != LocalConnectionState.Stopped)
                _networkManager.ServerManager.StopConnection(true);
            else
                _networkManager.ServerManager.StartConnection();

            _networkDiscovery.AdvertiseServer();*/
        }

        public async UniTask RequestServer()
        {
            _message.text = "Allocating server";
            _connecting = true;
            UpdateButtons();

            try
            {
                var joinCode = await StartHostWithRelay(2, "dtls");
                _yourCode.text = "YOUR CODE: " + joinCode;
                _yourCode.style.display = DisplayStyle.Flex;

                Debug.Log("STARTED SERVER " + joinCode);
                _connected = true;
            }
            catch (Exception ex)
            {
                _message.text = ex.Message;
                Debug.LogError(ex);
            }
            finally
            {
                _connecting = false;
            }

            UpdateButtons();
        }

        public async UniTask Join(string joinCode)
        {
            _message.text = "Allocating server";
            _connecting = true;
            UpdateButtons();

            try
            {
                await StartClientWithRelay(joinCode, "dtls");
                _yourCode.text = "YOUR CODE: " + joinCode;
                _yourCode.style.display = DisplayStyle.Flex;
                _connected = true;
            }
            catch (Exception ex)
            {
                _message.text = ex.Message;
                Debug.LogError(ex);
            }
            finally
            {
                _connecting = false;
            }

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            if (_connected)
            {
                _startServer.style.display = DisplayStyle.None;
                _connectContainer.style.display = DisplayStyle.None;
                _message.style.display = DisplayStyle.None;
                return;
            }

            _startServer.SetEnabled(!_connecting);
            _connect.SetEnabled(!_connecting);
        }

        public async UniTask<string> StartHostWithRelay(int maxConnections, string connectionType)
        {
            // Request allocation and join code
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            // Configure transport
            var unityTransport = _networkManager.TransportManager.GetTransport<FishyUnityTransport>();
            unityTransport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));

            // Start host
            if (_networkManager.ServerManager.StartConnection()) // Server is successfully started.
            {
                _networkManager.ClientManager
                    .StartConnection(); // You can choose not to call this method. Then only the server will start.
                return joinCode;
            }

            return null;
        }

        public async UniTask<bool> StartClientWithRelay(string joinCode, string connectionType)
        {
            var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
            _networkManager.GetComponent<FishyUnityTransport>()
                .SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
            return !string.IsNullOrEmpty(joinCode) && _networkManager.ClientManager.StartConnection();
        }

        public void OnClick_Client()
        {
            Join(_codeField.value).Forget();
        }


        private void StartServer()
        {
            OnClick_Server();
        }

        public void HideJoin()
        {
        }
    }
}