using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public bool shouldShowCountdown = false;
    public GameController gameController;
    public Text textCountDown;
    public GameObject healthContainer;

    public RectTransform timerRect;
    
    /** @todo I don't know how to get the width of the parent. I was going to use anchor instead but couldn't get it to work. so hard coding widht for now. */
    private float timerParentWidth = 100.0f;

    private UnityAction actionOnStartInitialCountdown;
    private UnityAction actionOnStartDayTime;
    private UnityAction actionOnHealthAction;
    private UnityAction actionOnStopDaytime;
    private UnityAction actionOnStartNightTime;

    private void Awake() {
        actionOnStartInitialCountdown += this.onStartInitialCountDown;
        actionOnStartDayTime += this.onStartDayTime;
        actionOnHealthAction += this.onHealthAction;
        actionOnStopDaytime += this.onStopDaytime;
        actionOnStartNightTime += this.onStartNightTime;

        gameController.StartListening(GameControllerEvents.START_INITIAL_COUNTDOWN, actionOnStartInitialCountdown);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnStartDayTime);
        gameController.StartListening(GameControllerEvents.HEALTH_ACTION, actionOnHealthAction);
        gameController.StartListening(GameControllerEvents.STOP_DAYTIME, actionOnStopDaytime);
        gameController.StartListening(GameControllerEvents.START_NIGHTTIME, actionOnStartNightTime);
    }

    public void onStartNightTime() {
        this.shouldShowCountdown = false;
    }

    public void onStopDaytime() {
        Debug.Log("Start night time amount ");
        Debug.Log(gameController.GetCurrCollectedNightTimePercent());
        timerRect.offsetMax = new Vector2(Mathf.Max(0.0f, Mathf.Min(1.0f, gameController.GetCurrCollectedNightTimePercent())) * (-this.timerParentWidth), timerRect.offsetMax.y);
        this.shouldShowCountdown = true;
    }

    public void onHealthAction () {
        CanvasRenderer[] allChildren = healthContainer.GetComponentsInChildren<CanvasRenderer>();

        float healthCount = gameController.pc.GetCurrHealth();

        Debug.Log(allChildren.Length);
        for (int i = 0; i < allChildren.Length; i++) {
            allChildren[i].SetAlpha(i < healthCount ? 1 : 0);
        }
    }
    
    public void onStartInitialCountDown() {
        shouldShowCountdown = true;
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString().PadLeft(4) : "0.0";

        timerRect.offsetMax = new Vector2(0.0f, timerRect.offsetMax.y);
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
