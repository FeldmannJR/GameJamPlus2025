using System;
using Codice.CM.Common.Tree;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;

namespace Scripts
{
    public class PressurePlate : NetworkBehaviour
    {
        private InteractableObject _interactable;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite ActiveSprite;
        public Sprite DeactivatedSprite;


        protected override void OnValidate()
        {
            if (!_spriteRenderer) return;
            UpdateState();
        }

        private Sprite GetCurrentSprite()
        {
            return _interactable.On ? ActiveSprite : DeactivatedSprite;
        }

        private void Start()
        {
            _interactable = GetComponent<InteractableObject>();
            _interactable.OnInteracted += OnInteracted;
            _spriteRenderer.sprite = GetCurrentSprite();
        }

        private void UpdateState()
        {
            _spriteRenderer.sprite = _interactable.On ? ActiveSprite : DeactivatedSprite;
        }

        public void OnInteracted()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                _interactable.On = true;
                UpdateState();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                _interactable.On = false;
                UpdateState();
            }
        }
    }
}