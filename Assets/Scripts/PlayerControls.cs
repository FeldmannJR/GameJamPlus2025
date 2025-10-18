using System;
using System.Collections.Generic;
using FishNet.Object;
using Scripts;
using Scripts.Helpers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class PlayerControls : NetworkBehaviour
    {
        public Transform GhostObject;


        public static PlayerControls LocalPlayer;
        public bool Finished = false;
        public static event Action<PlayerControls> OnStarted;
        public float MoveSpeed = 1f;
        //private GeneratedInputActions _inputActions;

        [SerializeField] private Collider2D _interactCollider;
        [SerializeField] private Rigidbody2D _rigidbody2D;


        private Cooldown _interactCooldown = new Cooldown(TimeSpan.FromMilliseconds(250));

        private Vector2 _moveDirection = Vector2.zero;


        public override void OnStartClient()
        {
            OnStarted?.Invoke(this);
            if (!IsOwner) return;
            LocalPlayer = this;
            RegisterControls();
        }


        private void RegisterControls()
        {
            var input = new InputActionsGenerated();
            input.Player.Enable();
            input.Player.Interact.performed += OnInteract;
            input.Player.Move.performed += OnMove;
            input.Player.Move.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext ev)
        {
            _moveDirection = ev.ReadValue<Vector2>();
        }


        private void FixedUpdate()
        {
            var position = this.transform.position;

            _rigidbody2D.MovePosition(position + _moveDirection.ToVector3() * Time.deltaTime * MoveSpeed);
            if (PositionConverterSystem.Instance.TryConvert(_rigidbody2D.position, out var ghostPos))
            {
                GhostObject.position = ghostPos;
            }
        }


        private void OnInteract(InputAction.CallbackContext ev)
        {
            try
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
            catch (Exception ex)
            {
                Debug.LogError(ex);
                throw ex;
            }
        }
    }
}