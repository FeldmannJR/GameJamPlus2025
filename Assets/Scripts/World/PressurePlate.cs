using System;
using Codice.CM.Common.Tree;
using DefaultNamespace;
using FishNet.Object;
using UnityEngine;

namespace Scripts
{
    public class PressurePlate : InteractableObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite ActiveSprite;
        public Sprite DeactivatedSprite;


        private void Start()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            _spriteRenderer.sprite = On ? ActiveSprite : DeactivatedSprite;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                SetOn(true);
                UpdateState();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<PlayerControls>(out _))
            {
                SetOn(false);
                UpdateState();
            }
        }
    }
}