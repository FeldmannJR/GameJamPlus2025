using System;
using DefaultNamespace;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(GameAudioBehaviour))]
    public class MusicController : MonoBehaviour
    {
        public static MusicController Instance;
        public GameAudioBehaviour _audioBehaviour;

        private void Awake()
        {
            Instance = this;
            _audioBehaviour = GetComponent<GameAudioBehaviour>();
        }

        private void Start()
        {
            LevelSystem.Instance.OnGameStart += OnGameStarted;

            SetMusic(false);
        }

        private void OnGameStarted()
        {
            SetMusic(true);
        }

        public void SetMusic(bool gamePlay)
        {
            _audioBehaviour.Value = gamePlay ? AudioEnum.GameTheme : AudioEnum.MainMenuTHeme;
            _audioBehaviour.PlayRepeating();
        }

        private float musicVOlume;
        public void ContinueMusic()
        {
            
            _audioBehaviour.Unpause();
        }

        public void PauseMusic()
        {
            _audioBehaviour.Pause();
        }
    }
}