using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    public bool shouldShowCountdown = false;
    public GameController gameController;
    public TMP_Text textCountDown;
    private HealthContainer healthContainer;

    public RectTransform timerRect;
    
    /** @todo I don't know how to get the width of the parent. I was going to use anchor instead but couldn't get it to work. so hard coding widht for now. */
    private float timerParentWidth = 100.0f;

    private UnityAction actionOnStartInitialCountdown;
    private UnityAction actionOnStartDayTime;
    private UnityAction actionOnHealthAction;
    private UnityAction actionOnStopDaytime;
    private UnityAction actionOnStartNightTime;
    private UnityAction actionOnStopNightTime;

    private void Awake() {
        actionOnHealthAction += this.onHealthAction;
        actionOnStartInitialCountdown += this.onStartInitialCountDown;
        actionOnStartDayTime += this.onStartDayTime;
        actionOnStopDaytime += this.onStopDaytime;
        actionOnStartNightTime += this.onStartNightTime;
        actionOnStopNightTime += this.onStopNightTime;

        gameController.StartListening(GameControllerEvents.HEALTH_ACTION, actionOnHealthAction);
        gameController.StartListening(GameControllerEvents.START_INITIAL_COUNTDOWN, actionOnStartInitialCountdown);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnStartDayTime);
        gameController.StartListening(GameControllerEvents.STOP_DAYTIME, actionOnStopDaytime);
        gameController.StartListening(GameControllerEvents.START_NIGHTTIME, actionOnStartNightTime);
        gameController.StartListening(GameControllerEvents.STOP_NIGHTTIME, actionOnStopNightTime);

        healthContainer = GetComponentInChildren<HealthContainer>();
    }
    private Vector2 GetNightTimeOffset() {
        return new Vector2(Mathf.Max(0.0f, Mathf.Min(1.0f, gameController.GetCurrCollectedNightTimePercent())) * (-this.timerParentWidth), timerRect.offsetMax.y);
    }

    private Vector2 GetDayTimeOffset() {
        return new Vector2(0.0f, timerRect.offsetMax.y);
    }

    public void onStartNightTime() {
        this.shouldShowCountdown = false;
    }

    public void onStopDaytime() {
        timerRect.offsetMax = this.GetNightTimeOffset();
        this.shouldShowCountdown = true;
    }

    public void onStopNightTime() {
        timerRect.offsetMax = this.GetDayTimeOffset();
        this.shouldShowCountdown = true;
    }

    public void onHealthAction () {
        float healthCount = gameController.pc.GetCurrHealth();
        healthContainer.SetHealth((int)healthCount);
    }
    
    public void onStartInitialCountDown() {
        shouldShowCountdown = true;
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString().PadLeft(4) : "0.0";

        timerRect.offsetMax = this.GetDayTimeOffset();
    }

    public void onStartDayTime() {
        shouldShowCountdown = false;
    }

    private void FixedUpdate() {
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString() : "";

        if (!shouldShowCountdown) {
           timerRect.offsetMax = new Vector2(gameController.GetTimeProgress() * -this.timerParentWidth, timerRect.offsetMax.y);
        }
    }
}
