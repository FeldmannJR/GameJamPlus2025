using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class LeverGroup : InteractableObject
    {
        [SerializeField] private List<Lever> _levers;
        [SerializeField] private float _delayAfterCombinationFinished;
        private int _currentIndex = 0;
        private bool _failed = false;

        private void Start()
        {
            foreach (var lever in _levers)
            {
                lever.OnStateChanged += () => OnLever(lever);
            }
        }

        private async UniTaskVoid OnFinishedCombination(bool failed)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delayAfterCombinationFinished));
            if (failed)
            {
                foreach (var lever in _levers)
                {
                    lever.SetOn(false);
                }

                _failed = false;
                _currentIndex = 0;
                return;
            }

            SetOn(true);
        }

        private void OnLever(Lever lever)
        {
            if (_currentIndex >= _levers.Count)
            {
                return;
            }


            var currentOne = _levers[_currentIndex];
            if (currentOne != lever)
            {
                _failed = true;
            }

            _currentIndex++;
            if (_currentIndex >= _levers.Count)
            {
                OnFinishedCombination(_failed).Forget();
                return;
            }
        }
    }
}