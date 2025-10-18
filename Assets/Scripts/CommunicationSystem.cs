using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CommunicationSystem : MonoBehaviour
    {
        public static CommunicationSystem Instance;

        public string GetLocalPlayerName()
        {
            return "Local";
        }

        private void Awake()
        {
            Instance = this;
        }

        public event Action<string> OnMessage;

        public void SendTextMessage(string str)
        {
            OnMessage?.Invoke(GetLocalPlayerName() + ": " + str);
            // TODO FISH
        }
    }
}