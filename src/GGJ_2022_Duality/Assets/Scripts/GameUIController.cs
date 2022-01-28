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
    private UnityAction actionOnStartInitialCountdown;
    private UnityAction actionOnStartDayTime;

    private void Awake() {
        actionOnStartInitialCountdown += this.onStartInitialCountDown;
        actionOnStartDayTime += this.onStartDayTime;

        gameController.StartListening(GameControllerEvents.START_INITIAL_COUNTDOWN, actionOnStartInitialCountdown);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, actionOnStartDayTime);
    }
    
    public void onStartInitialCountDown() {
        shouldShowCountdown = true;
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString().PadLeft(4) : "0.0";
    }

    public void onStartDayTime() {
        shouldShowCountdown = false;
    }

    private void FixedUpdate() {
        textCountDown.text = shouldShowCountdown ? gameController.GetTime().ToString() : "";
    }
}
