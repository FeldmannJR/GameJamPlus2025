using System;
using Codice.CM.Common.Tree;
using FishNet.Object;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(InteractableObject))]
    public class Lever : NetworkBehaviour
    {
        private InteractableObject _interactable;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public Sprite ActiveSprite;
        public Sprite DeactivatedSprite;


        protected override void OnValidate()
        {
            if (!_spriteRenderer) return;
            _spriteRenderer.sprite = GetCurrentSprite();
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


        public void OnInteracted()
        {
            Debug.Log("INTERACTED");
            _interactable.On = !_interactable.On;
            _spriteRenderer.sprite = GetCurrentSprite();
        }
    }
}