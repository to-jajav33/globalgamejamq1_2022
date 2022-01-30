using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerVisuals pv;

    private Rigidbody2D body;
    private BoxCollider2D boxCol;

    [SerializeField]
    private LayerMask groundLayermask;

    private bool isFlying = false;
    private bool canMove = false;
    private bool isJumping = false;

    [SerializeField]
    private float jumpForce = 350f;
    private float moveForce = 16f;

    private float currHealth = 3;
    private float maxHealth = 3;

    private const float normalGravity = 17f;
    private const float flyingGravity = -1 * normalGravity/2f;

    private GameController gameController;

    private UnityAction actionOnDayTime;
    private UnityAction actionOnDayTimeStop;
    private UnityAction actionOnNightTime;
    private UnityAction actionOnNightTimeStop;

    public Action OnHealthAction = delegate { };
    public Action OnDeath = delegate { };

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        gameController = FindObjectOfType<GameController>();
        pv = GetComponentInChildren<PlayerVisuals>();

        gameController.SetPlayerController(this);

        actionOnDayTime += this.OnDaytime;
        actionOnDayTimeStop += this.OnDaytimeStop;
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnDayTime);
        gameController.StartListening(GameControllerEvents.STOP_DAYTIME, actionOnDayTimeStop);
        actionOnNightTime += this.OnNightTime;
        actionOnNightTimeStop += this.OnNightTimeStop;
        
        gameController.StartListening(GameControllerEvents.START_NIGHTTIME, actionOnNightTime);
        gameController.StartListening(GameControllerEvents.STOP_NIGHTTIME, actionOnNightTimeStop);
    }

    private void Start()
    {
        body.gravityScale = normalGravity;
    }

    private void OnNightTime () {
        pv.SetAnimatorState(PlayerAniState.SLEEP, false);
        pv.SetAnimatorState(PlayerAniState.FLY, true);
        SetCanMove(true);
        isFlying = true;
        ToggleGravity();
    }

    private void OnNightTimeStop() {
        pv.SetAnimatorState(PlayerAniState.FLY, false);
        pv.SetAnimatorState(PlayerAniState.SLEEP, true);
        SetCanMove(false);
        ToggleGravity();
        isFlying = false;
    }

    private void OnDaytime () {
        pv.SetAnimatorState(PlayerAniState.SLEEP, false);
        SetCanMove(true);
        isFlying = false;
    }

    private void OnDaytimeStop() {
        SetCanMove(false);
        pv.SetAnimatorState(PlayerAniState.SLEEP, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(((1 << collision.gameObject.layer) & groundLayermask) != 0)
        {
            if (isJumping)
            {
                Debug.Log("Hit Ground!");
                isJumping = false;
                pv.SetAnimatorState(PlayerAniState.JUMP, isJumping);
            }
        }
    }

    private void ToggleGravity()
    {
        if (body.gravityScale < 0)
        {
            body.gravityScale = normalGravity;
        }
        else
        {
            body.gravityScale = flyingGravity;
        }

    }

    private void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
        pv.SetAnimatorState(PlayerAniState.MOVE, canMove);
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

    public void JumpAction()
    {

        if (isFlying)
        {
            FlyJump();
        }
        else
        {
            GroundJump();
        }
    }

    private void GroundJump()
    {
        if (IsGrounded())
        {
            isJumping = true;
            pv.SetAnimatorState(PlayerAniState.JUMP, isJumping);
            float moveInput = canMove ? 1 : 0;
            Vector2 jumpVector = moveInput * Vector2.up * jumpForce;
            body.AddForce(jumpVector, ForceMode2D.Impulse);
            //body.velocity = new Vector2(body.velocity.x, moveInput * jumpForce);
            //body.velocity = Vector2.up * jumpForce;
        }
    }

    private void FlyJump()
    {
        float moveInput = canMove ? 1 : 0;
        //Vector2 jumpVector = moveInput * Vector2.down * (3f * jumpForce/4f);
        //body.AddForce(jumpVector, ForceMode2D.Impulse);
        body.velocity = new Vector2(body.velocity.x, -1 * moveInput * jumpForce / 2f);
        //body.velocity = Vector2.up * jumpForce;
    }

    public float GetCurrHealth()
    {
        return this.currHealth;
    }

    private bool IsAgainstWall()
    {
        Vector3 actualSize = boxCol.bounds.size + new Vector3(boxCol.edgeRadius, boxCol.edgeRadius, boxCol.edgeRadius);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, actualSize, 0f, Vector2.right, actualSize.x/4, groundLayermask);
        return raycastHit.collider != null;
    }

    private bool IsGrounded()
    {
        Vector3 actualSize = boxCol.bounds.size + new Vector3(boxCol.edgeRadius, boxCol.edgeRadius, boxCol.edgeRadius);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, actualSize, 0f, Vector2.down, actualSize.y/4, groundLayermask);
        return raycastHit.collider != null;
    }

    public void HurtPlayer(int amount = 1)
    {
        Debug.Log("Player Hurt!");
        currHealth -= amount;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        OnHealthAction?.Invoke();

        if (currHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        StartCoroutine(DelayGameOverTransition());

        body.velocity = Vector2.zero;
        SetCanMove(false);
        pv.SetAnimatorState(PlayerAniState.DEAD, true);
    }

    private IEnumerator DelayGameOverTransition()
    {
        yield return new WaitForSeconds(1f);
        OnDeath.Invoke();
    }

    public void HealPlayer(int amount = 1)
    {
        Debug.Log("Player Healed!");
        currHealth += amount;
        if (currHealth > maxHealth)
        {
            gameController.AddScore(5);
        }
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        OnHealthAction?.Invoke();
    }
}