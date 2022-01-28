using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private CircleCollider2D cirCol;

    [SerializeField]
    private LayerMask groundLayermask;

    private bool isFlying = false;
    private bool canMove = false;

    [SerializeField]
    private float jumpForce = 500f;
    private float moveForce = 25f;

    private float currHealth = 3;
    private float maxHealth = 3;

    private GameController gameController;

    private UnityAction actionOnDayTime;

    public Action OnHealthAction = delegate { };

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        cirCol = GetComponent<CircleCollider2D>();
        gameController = FindObjectOfType<GameController>();

        actionOnDayTime += this.OnDaytime;
        
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnDayTime);
    }

    private void OnDaytime () {
        canMove = true;
        isFlying = false;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (!IsAgainstWall())
            {
                body.velocity = new Vector2(moveForce, body.velocity.y);
            }
            else
            {
                body.velocity = new Vector2(0, body.velocity.y);
            }

        }
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
            float moveInput = canMove ? 1 : 0;
            Vector2 jumpVector = moveInput * Vector2.up * jumpForce;
            body.AddForce(jumpVector, ForceMode2D.Impulse);
            //body.velocity = new Vector2(body.velocity.x, moveInput * jumpForce);
            //body.velocity = Vector2.up * jumpForce;
        }
    }

    private void FlyJump(bool isJumping)
    {
        if (isJumping)
        {
            float moveInput = canMove ? 1 : 0;
            Vector2 jumpVector = moveInput * Vector2.up * jumpForce;
            body.AddForce(jumpVector, ForceMode2D.Impulse);
            //body.velocity = new Vector2(body.velocity.x, moveInput * jumpForce);
            //body.velocity = Vector2.up * jumpForce;
        }

    }

    private bool IsAgainstWall()
    {
        RaycastHit2D raycastHit = Physics2D.CircleCast(cirCol.bounds.center, cirCol.radius / 4,  Vector2.right, cirCol.radius, groundLayermask);
        return raycastHit.collider != null;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.CircleCast(cirCol.bounds.center, cirCol.radius/2, Vector2.down, 1f, groundLayermask);
        return raycastHit.collider != null;
    }

    public void HurtPlayer(int amount = 1)
    {
        Debug.Log("Player Hurt!");
        currHealth -= amount;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        OnHealthAction?.Invoke();
    }

    public void HealPlayer(int amount = 1)
    {
        Debug.Log("Player Healed!");
        currHealth += amount;
        if (currHealth > maxHealth)
        {
            //!TODO: Add extra points here
        }
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        OnHealthAction?.Invoke();
    }
}
