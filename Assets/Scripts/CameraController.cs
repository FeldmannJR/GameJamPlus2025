using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        public PlayerControls _player;
        private bool _playerSet = false;


        private void Start()
        {
            PlayerControls.OnStartedOwner += OnOwnerSet;
        }

        private void OnOwnerSet(PlayerControls obj)
        {
            _playerSet = true;
            _player = obj;
        }

        private void Update()
        {
            if (_playerSet)
            {
                var pos =  _player.transform.position;
                pos.z = -10;
                this.transform.position = pos;
            }
        }
    }
}