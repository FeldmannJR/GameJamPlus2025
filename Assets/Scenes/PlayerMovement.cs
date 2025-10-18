/*using System;
using FishNet.Demo.AdditiveScenes;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    private InputSystem_Actions _actions;
    private PlayerInput _input;

    private Vector2 _velocity;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
    }

    public override void OnStartClient()
    {
        if (!IsOwner)
        {
            return;
        }

        _actions.Enable();
        _actions.Player.Move.performed += OnMoved;
    }


    private void OnMoved(InputAction.CallbackContext obj)
    {
        _velocity = obj.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        var newPosition = new Vector2(this.transform.position.x, this.transform.position.y) +
                          _velocity * Time.deltaTime;
        this.transform.position = newPosition;
    }
}*/