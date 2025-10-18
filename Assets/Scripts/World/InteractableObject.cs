using System;
using System.Collections.Generic;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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

        public static int _interactIndex = 0;

        private Vector2 _velocity;

        public event Action OnInteracted;
        public event Action OnStateChanged;


        public List<InteractableObject> Triggers;

        private int _lastProcessedEvent = 0;


        public void OnLocalPlayerInteract()
        {
            RpcInteract();
        }

        [ObserversRpc]
        public void RpcInteract()
        {
            Debug.Log("RPC INTERACTED");
            Interact(++_interactIndex);
        }


        /// <summary>
        /// If source = null then its the player
        /// </summary>
        /// <param name="source"></param>
        public void Interact(int eventIndex)
        {
            // Prevents trigger recursion
            if (eventIndex <= _lastProcessedEvent)
            {
                return;
            }

            _lastProcessedEvent = eventIndex;

            OnInteracted?.Invoke();
            foreach (var trigger in Triggers)
            {
                trigger.Interact(eventIndex);
            }
        }
    }
}