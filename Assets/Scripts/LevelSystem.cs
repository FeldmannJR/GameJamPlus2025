using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance;

        public List<PlayerControls> _players = new();

        public event Action<PlayerControls> OnPlayerFinish;
        public event Action OnWin;
        public GameEndUI _gameEnd;

        public event Action OnGameStart;
        public bool GameStarted = false;

        private int _won = 0;

        private void Awake()
        {
            Instance = this;
            PlayerControls.OnStarted += OnPlayerStarted;
            PlayerControls.OnStopped += OnPlayerStopped;
        }

        private void OnPlayerStopped(PlayerControls obj)
        {
            _players.Remove(obj);
        }

        private void OnPlayerStarted(PlayerControls obj)
        {
            _players.Add(obj);
            obj.Freeze = true;
            CheckGameStart();
        }

        public void CheckGameStart()
        {
            if (_players.Count == 2)
            {
                StartGame();
            }
        }

        public void StartGame()
        {
            GameStarted = true;
            foreach (var playerControlse in this._players)
            {
                playerControlse.Freeze = false;
            }

            UIService.Instance.ShowGameplay(true);
            OnGameStart?.Invoke();
        }

        public void Finish(PlayerControls player)
        {
            if (player.Finished) return;
            player.Finished = true;
            _won++;
            OnPlayerFinish?.Invoke(player);
            if (_won == 2)
            {
                Win();
                return;
            }
        }


        public void Win()
        {
            OnWin?.Invoke();
            UIService.Instance.ShowGameplay(false);
            UIService.Instance.ShowGameEnd(true);
            _gameEnd.Show("Win");
        }
    }
}