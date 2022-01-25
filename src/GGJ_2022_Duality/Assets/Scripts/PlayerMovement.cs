using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Input input;

    // Start is called before the first frame update
    private void Awake()
    {
        this.input = new Input();
    }

    private void OnDisable() {
        input.Disable();

        input.Player.Jump.started -= OnJump;
        input.Player.Jump.canceled -= OnJump;
    }

    private void OnEnable() {
        input.Enable();
    }

    private void Start() {
        input.Player.Jump.started += OnJump;
        input.Player.Jump.canceled += OnJump;
    }

    private void OnJump(InputAction.CallbackContext ctx) {
        if (ctx.ReadValue<float>() == 1) {
            Debug.Log("Jump");
        } else {
            Debug.Log("Jump cancelled");
        }
    }
}
