using System;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class UIService : MonoBehaviour
    {
        public static UIService Instance;
        public GameplayUI Gameplay;
        public ConnectUI Connect;
        public GameEndUI GameEnd;
        public FrameUI FrameUI;

        private void Awake()
        {
            Instance = this;
            PlayerControls.OnStarted += OnPlayerStarted;
        }

        private void OnPlayerStarted(PlayerControls obj)
        {
            if (obj.IsOwner)
            {
            }
        }


        public void ShowGameplay(bool show)
        {
            Gameplay.gameObject.SetActive(show);
        }

        public void ShowGameEnd(bool b)
        {
            GameEnd.gameObject.SetActive(b);
        }

        public void ShowFrame(Sprite frame, Action onConfirm)
        {
            FrameUI.gameObject.SetActive(true);
            FrameUI.SetData(frame, onConfirm);
        }
    }
}