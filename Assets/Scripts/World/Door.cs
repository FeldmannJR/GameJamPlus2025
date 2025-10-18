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


        [SerializeField] private bool _stayOpen = false;
        public Sprite OpenSprite;
        public Sprite ClosedSprite;


        private void Start()
        {
        }

        public void SetOpen(bool open)
        {
            Open = open;
            _spriteRenderer.sprite = Open ? OpenSprite : ClosedSprite;
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
            var open = RequiredToOpen.All(req => req.On);
            if (!open && _stayOpen) return;
            SetOpen(open);
        }
    }
}