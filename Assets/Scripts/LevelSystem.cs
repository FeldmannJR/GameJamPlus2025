using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance;

        public List<PlayerControls> _players;

        public event Action<PlayerControls> OnPlayerFinish;
        public event Action OnWin;
        public GameEndUI _gameEnd;


        private int _won = 0;

        private void Awake()
        {
            Instance = this;
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
            _gameEnd.Show("Win");
        }
    }
}