using System;
using System.Collections.Generic;
using FishNet.Object;
using ParrelSync;
using Scripts;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerControls : NetworkBehaviour
    {
        public static event Action<PlayerControls> OnStartedOwner;
        public float MoveSpeed = 1f;
        //private GeneratedInputActions _inputActions;

        [SerializeField] private Collider2D _interactCollider;
        [SerializeField] private Rigidbody2D _rigidbody2D;


        private Cooldown _interactCooldown = new Cooldown(TimeSpan.FromMilliseconds(250));

        private Vector2 _moveDirection = Vector2.zero;


        public override void OnStartClient()
        {
            if (!IsOwner) return;
            OnStartedOwner?.Invoke(this);
            /*_inputActions = new GeneratedInputActions();
            if (ClonesManager.IsClone())
            {
                _inputActions = new GeneratedInputActions();

            }
            _inputActions.Enable();
            _inputActions.Player.Interact.performed += OnInteract;
            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMove;*/
        }

        private void OnMove(InputAction.CallbackContext ev)
        {
            _moveDirection = ev.ReadValue<Vector2>();
        }


        private void FixedUpdate()
        {
            var position = this.transform.position;

            _rigidbody2D.MovePosition(position + _moveDirection.ToVector3() * Time.deltaTime * MoveSpeed);
        }


        private void OnInteract(InputAction.CallbackContext ev)
        {
            Debug.Log("On interact");
            var filter = new ContactFilter2D();
            filter.SetLayerMask(PhysicsLayers.Mask_Interactable);
            filter.useTriggers = true;

            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(_interactCollider, filter, colliders);


            var closest = float.MaxValue;
            InteractableObject found = null;
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<InteractableObject>(out var obj))
                {
                    var distance = Vector2.Distance(col.bounds.center, transform.position);
                    if (distance < closest)
                    {
                        found = obj;
                        closest = distance;
                    }
                }
            }

            if (found != null && _interactCooldown.CheckTrigger())
            {
                found.OnLocalPlayerInteract();
            }
        }
    }
}