using System;
using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using UnityEngine;

namespace Scripts
{
    public class Door : NetworkBehaviour
    {
        public List<InteractableObject> RequiredToOpen;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _colliderObject;
        public bool Open = false;

        public Sprite OpenSprite;
        public Sprite ClosedSprite;

        protected override void OnValidate()
        {
            if (!_spriteRenderer) return;
            SetOpen(Open);
        }


        public void SetOpen(bool open)
        {
            Open = open;
            _spriteRenderer.sprite = Open ? OpenSprite : ClosedSprite;
            _colliderObject.SetActive(!open);
        }


        private void Start()
        {
            foreach (var interactableObject in RequiredToOpen)
            {
                interactableObject.OnStateChanged += UpdateState;
            }
        }

        public void UpdateState()
        {
            var open = RequiredToOpen.All(req => req.On);
            SetOpen(open);
        }


        public void OnInteracted()
        {
            Debug.Log("INTERACTED");
            SetOpen(!Open);
        }
    }
}