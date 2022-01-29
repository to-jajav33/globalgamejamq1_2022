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

    private UnityAction actionOnStartInitialCountdown;
    private UnityAction actionOnStartDayTime;
    private UnityAction actionOnHealthAction;

    private void Awake() {
        actionOnStartInitialCountdown += this.onStartInitialCountDown;
        actionOnStartDayTime += this.onStartDayTime;
        actionOnHealthAction += this.onHealthAction;

        gameController.StartListening(GameControllerEvents.START_INITIAL_COUNTDOWN, actionOnStartInitialCountdown);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnStartDayTime);
        gameController.StartListening(GameControllerEvents.HEALTH_ACTION, actionOnHealthAction);
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

        timerRect.offsetMax = new Vector2(0.0f, timerRect.offsetMax.y);
    }

    private void FixedUpdate() {
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString() : "";

        if (!shouldShowCountdown) {
           timerRect.offsetMax = new Vector2(gameController.GetTimeProgress() * -100.0f, timerRect.offsetMax.y);
        }
    }
}
