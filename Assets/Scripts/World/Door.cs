using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class Door : NetworkBehaviour
    {
        public List<InteractableObject> RequiredToOpen;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _colliderObject;
        [SerializeField] private GameAudioBehaviour _unlock;
        public bool Open = false;


        [SerializeField] private bool _stayOpen = false;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;


        public UnityEvent OnOpen;

        private void Start()
        {
        }

        public void SetOpen(bool open)
        {
            if (open != Open && Time.time > 3f) // hack for not playing when starting
            {
                _unlock.PlayOneShot();
            }

            Open = open;
            _spriteRenderer.color = !Open ? Color.white : new Color(1, 1, 1, 0.15f);
            _colliderObject.SetActive(!open);
        }


        private void OnEnable()
        {
            foreach (var interactableObject in RequiredToOpen)
            {
                interactableObject.OnStateChanged += UpdateState;
            }

            SetOpen(false);
        }

        public void UpdateState()
        {
            var hasRequirements = RequiredToOpen.All(req => req.On);
            if (!hasRequirements && _stayOpen) return;
            SetOpen(hasRequirements);
            if (hasRequirements)
            {
                OnOpen?.Invoke();
            }
        }
    }
}