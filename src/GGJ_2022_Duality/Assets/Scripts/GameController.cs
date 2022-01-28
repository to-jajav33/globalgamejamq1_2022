using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.UI;

public class GameControllerEvents {
    public static string HEALTH_ACTION = "HEALTH_ACTION";
    public static string START_INITIAL_COUNTDOWN = "START_INITIAL_COUNTDOWN";
    public static string START_DAYTIME = "START_DAYTIME";
    public static string STOP_DAYTIME = "STOP_DAYTIME";
    public static string START_NIGHTTIME = "START_NIGHTTIME";
    public static string STOP_NIGHTTIME = "STOP_NIGHTTIME";
    public static string HIT_TRAP ="HIT_TRAP";
    public static string COLLECTED_NIGHTTIME = "COLLECTED_NIGHTTIME";
    public static string LOST_NIGHTTIME = "LOST_NIGHTTIME";
    public static string GAME_OVER = "GAME_OVER";
}

public class GameController : MonoBehaviour
{
    public EventManager eventManager;
    public  PlayerController pc;

    private float BEGINNING_MAX_COUNTDOWN = 3.0f;
    private float DAY_TIME_MAX_COUNTDOWN = 10.0f; // in seconds
    private float AMOUNT_OF_TIME_PER_COLLECTED_NIGHTTIME = 0.01f;

    private int score = 0;
    private int currCollectedNightTime = 0;
    private float time = 10.0f; // in seconds
    private bool isTimerRunning = false;
    private bool isDayTime = false;
    private bool isStartCountdown = false;


    private void OnEnable() {
        if (!(eventManager is EventManager)) {
            this.eventManager = FindObjectOfType<EventManager>();

            if (!(eventManager is EventManager)) {
                Debug.LogError("Could not find Event Manager");
            }
        }
    }

    private void FixedUpdate() {
        //Debug.Log(this.time);
    }

    public float GetTime() {
        return Mathf.Ceil(this.time * 10.0f) / 10.0f;
    }

    private void Update() {
        if (this.isTimerRunning) {
            this.time -= Time.deltaTime;

            if (this.time <= 0.0f) {
                if (this.isStartCountdown) {
                    this.TriggerDayTimeStart();
                } else if (this.isDayTime) {
                    this.TriggerDayTimeStop();
                } else {
                    this.TriggerNightTimeStop();
                }
            }
        }
    }

    private void Start() {
        // call this after everything is setup... for now just doing it immediately
        this.TriggerInitialCountdownStart();
    }

    private void OnDisable() {
        FieldInfo[] properties = typeof(GameControllerEvents).GetFields();

        foreach (FieldInfo prop in properties) {
            // https://stackoverflow.com/questions/12480279/iterate-through-properties-of-static-class-to-populate-list
            this.eventManager.StopListening(prop.GetValue(null).ToString());
        }
    }

    public int GetScore() {
        return this.currCollectedNightTime + this.score;
    }

    public void SetPlayerController(PlayerController paramPC) {
        this.pc = paramPC;
        this.pc.OnHealthAction += this.TriggerHealthaction;
    }

    public void TriggerHealthaction() {
        this.eventManager.TriggerEvent(GameControllerEvents.HEALTH_ACTION);
    }

    public void TriggerInitialCountdownStart() {
        this.time = this.BEGINNING_MAX_COUNTDOWN;
        this.isStartCountdown = true;
        this.isTimerRunning = true;
        this.eventManager.TriggerEvent(GameControllerEvents.START_INITIAL_COUNTDOWN);
    }

    public void TriggerDayTimeStart() {
        this.isStartCountdown = false;
        this.isDayTime = true;
        this.time = this.DAY_TIME_MAX_COUNTDOWN;
        this.isTimerRunning = true;
        this.eventManager.TriggerEvent(GameControllerEvents.START_DAYTIME);
    }

    public void TriggerDayTimeStop() {
        this.time = 0.0f;
        this.isTimerRunning = false;
        this.eventManager.TriggerEvent(GameControllerEvents.STOP_DAYTIME);
    }

    public void TriggerNightTimeStart() {
        this.isDayTime = false;
        this.time = this.currCollectedNightTime * this.AMOUNT_OF_TIME_PER_COLLECTED_NIGHTTIME;
        this.isTimerRunning = true;
        this.eventManager.TriggerEvent(GameControllerEvents.START_NIGHTTIME);
    }

    public void TriggerNightTimeStop() {
        this.score += this.currCollectedNightTime;
        this.currCollectedNightTime = 0;

        this.time = 0.0f;
        this.isTimerRunning = false;

        this.eventManager.TriggerEvent(GameControllerEvents.STOP_NIGHTTIME);
    }

    public void TriggerGameOver() {
        this.eventManager.TriggerEvent(GameControllerEvents.GAME_OVER);
    }

    public void TriggerPlayerHitTrap() {
        this.eventManager.TriggerEvent(GameControllerEvents.HIT_TRAP);
    }

    public void TriggerPlayerCollectedMoreNighttime() {
        this.currCollectedNightTime++;
        this.eventManager.TriggerEvent(GameControllerEvents.COLLECTED_NIGHTTIME);
    }

    public void TriggerPlayerLostNighttime() {
        if (this.currCollectedNightTime > 0) {
            this.currCollectedNightTime--;
        }
        this.eventManager.TriggerEvent(GameControllerEvents.LOST_NIGHTTIME);
    }

    public void StartListening(string evName, UnityAction listener) {
        this.eventManager.StartListening(evName, listener);
    }

    /**
    * Pass in just the string, no listener to remove all listeners
    */
    public void StopListening(string evName, UnityAction listener = null) {
        this.eventManager.StopListening(evName, listener);
    }
}
