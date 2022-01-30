using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Animator ani;

    private string moveStr = "isMoving";
    private string flyStr = "isFlying";
    private string sleepStr = "isSleeping";
    private string deadStr = "isDead";
    private string jumpStr = "isJumping";

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    public void SetAnimatorState(PlayerAniState playerAniState, bool isOn)
    {
        switch (playerAniState)
        {
            case PlayerAniState.MOVE:
                SetAnimatorBool(moveStr, isOn);
                break;
            case PlayerAniState.FLY:
                SetAnimatorBool(flyStr, isOn);
                break;
            case PlayerAniState.SLEEP:
                ani.SetTrigger("startSleep");
                SetAnimatorBool(sleepStr, isOn);
                break;
            case PlayerAniState.DEAD:
                ani.SetTrigger(deadStr);
                break;
            case PlayerAniState.JUMP:
                SetAnimatorBool(jumpStr, isOn);
                break;
        }
    }

    private void SetAnimatorBool(string str, bool isOn)
    {
        ani.SetBool(str, isOn);
    }
}

public enum PlayerAniState
{
    MOVE,
    FLY,
    SLEEP,
    DEAD,
    JUMP
}
