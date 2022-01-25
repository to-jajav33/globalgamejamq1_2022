using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private CircleCollider2D cirCol;

    [SerializeField]
    private LayerMask groundLayermask;

    private bool isFlying = false;

    [SerializeField]
    private float jumpForce = 50f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        cirCol = GetComponent<CircleCollider2D>();
    }

    public void JumpAction(bool isJumping)
    {
        if (isFlying)
        {
            FlyJump(isJumping);
        }
        else
        {
            GroundJump(isJumping);
        }
    }

    private void GroundJump(bool isJumping)
    {
        if (isJumping && IsGrounded())
        {
            body.velocity = Vector2.up * jumpForce;
        }
    }

    private void FlyJump(bool isJumping)
    {
        if (isJumping)
        {
            body.velocity = Vector2.up * jumpForce;
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.CircleCast(cirCol.bounds.center, cirCol.radius, Vector2.down, 1f, groundLayermask);
        return raycastHit.collider != null;
    }
}
