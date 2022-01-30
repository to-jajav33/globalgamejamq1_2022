using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    private Input input;
    private bool canInput;

    private PlayerController pc;

    private void Awake()
    {
        this.input = new Input();
        pc = GetComponent<PlayerController>();
    }

    private void OnDisable()
    {
        input.Disable();

        input.Player.Jump.started -= OnJump;
        input.Player.Jump.canceled -= OnJump;
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;
    }

    private void Start()
    {

    }

    private void SetInput(bool _canInput)
    {
        canInput = _canInput;
        if (canInput)
        {
            input.Enable();
        }
        else
        {
            input.Disable();
        }
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        pc.JumpAction();
        /*
        if (ctx.ReadValue<float>() == 1)
        {
            pc.JumpAction(true);
        }
        else
        {
            pc.JumpAction(false);
        }*/
    }
}
