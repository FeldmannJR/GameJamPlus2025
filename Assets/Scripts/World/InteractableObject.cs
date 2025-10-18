using System;
using System.Collections.Generic;
using DefaultNamespace;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class InteractableObject : NetworkBehaviour
    {
        [SerializeField] private bool _on = false;

        public bool On
        {
            get => _on;
            set
            {
                _on = value;
                OnStateChanged?.Invoke();
            }
        }


        private Vector2 _velocity;

        public event Action OnInteracted;
        public event Action OnStateChanged;


        private int _lastProcessedEvent = 0;


        public void OnLocalPlayerInteract()
        {
            Debug.Log("SENDING RPC");
            RpcInteract();
        }


        [ServerRpc(RequireOwnership = false)]
        public void RpcInteract()
        {
            Debug.Log("RPC INTERACTED");
            RpcObInteract();
        }

        [ObserversRpc]
        public void RpcObInteract()
        {
            Interact();
        }


        public void SetOn(bool b)
        {
            On = (b);
        }


        /// <summary>
        /// If source = null then its the player
        /// </summary>
        /// <param name="source"></param>
        public void Interact()
        {
            OnInteracted?.Invoke();
        }
    }
}