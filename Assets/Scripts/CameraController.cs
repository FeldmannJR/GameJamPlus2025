using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private LevelSystem _level;
        public PlayerControls _player;
        private bool _playerSet = false;

        private float _startZ;

        private void Start()
        {
            _startZ = this.transform.position.z;
            PlayerControls.OnStarted += OnSet;
            _level.OnPlayerFinish += OnPlayerFinish;
        }

        private void OnPlayerFinish(PlayerControls finished)
        {
            if (!finished.IsOwner) return;
            foreach (var pl in _level._players)
            {
                if (!pl.IsOwner && !pl.Finished)
                {
                    SetTarget(finished);
                }
            }
        }

        private void OnSet(PlayerControls obj)
        {
            if (obj.IsOwner)
            {
                SetTarget(obj);
            }
        }


        public void SetTarget(PlayerControls player)
        {
            _playerSet = true;
            _player = player;
        }

        public void OnPlayerFinished()
        {
        }

        private void Update()
        {
            if (_playerSet)
            {
                var pos = _player.transform.position;
                pos.z = _startZ;
                this.transform.position = pos;
            }
        }
    }
}