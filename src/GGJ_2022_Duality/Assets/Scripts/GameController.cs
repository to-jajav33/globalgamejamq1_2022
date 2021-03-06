using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using UnityEngine.UI;
using System;

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
    private static GameController instance;
    public static GameController Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }
            return instance;
        }
    }

    public EventManager eventManager;
    public  PlayerController pc;

    private SceneTransitionController stc => SceneTransitionController.Instance;

    private float BEGINNING_MAX_COUNTDOWN = 3.0f;
    private float DAY_TIME_MAX_COUNTDOWN = 10.0f; // in seconds
    private float AMOUNT_OF_TIME_PER_COLLECTED_NIGHTTIME = 0.01f;

    private int score = 0;
    private int currCollectedNightTime = 0;
    private float time = 10.0f; // in seconds
    private bool isTimerRunning = false;
    private bool isDayTime = false;
    private bool isStartCountdown = false;
    private bool isTransitionCountdown = false;

    public Action<int> NewScoreEvent = delegate { };
    public Action<bool> OnDayTransition = delegate { };

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this); }
        else { instance = this; }

        pc = FindObjectOfType<PlayerController>();
    }

    private void OnEnable() {
        if (!(eventManager is EventManager)) {
            this.eventManager = FindObjectOfType<EventManager>();

            if (!(eventManager is EventManager)) {
                Debug.LogError("Could not find Event Manager");
            }
        }

        pc.OnDeath += GameOver;
    }

    private void GameOver()
    {
        GameObject tempData = new GameObject();
        TempLevelData tld = tempData.AddComponent<TempLevelData>();
        tld.intData = score;

        stc.LoadGameOver();
    }

    private void FixedUpdate() {
        //Debug.Log(this.time);
    }

    public float GetTime() {
        return Mathf.Ceil(this.time * 10.0f) / 10.0f;
    }

    public float GetTimeProgress() {
        return (this.DAY_TIME_MAX_COUNTDOWN - this.GetTime()) / this.DAY_TIME_MAX_COUNTDOWN;
    }

    private void Update() {
        if (this.isTimerRunning) {
            this.time -= Time.deltaTime;

            if (this.time <= 0.0f) {
                if (this.isStartCountdown) {
                    this.TriggerDayTimeStart();
                } else if (this.isTransitionCountdown) {
                    if (this.isDayTime) {
                        this.TriggerNightTimeStart();
                    } else {
                        this.TriggerDayTimeStart();
                    }
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

    public void AddScore(int _amount)
    {
        score += _amount;
        NewScoreEvent?.Invoke(score);
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
        this.isTransitionCountdown = false;
        this.isStartCountdown = false;
        this.isDayTime = true;
        this.time = this.DAY_TIME_MAX_COUNTDOWN;
        this.isTimerRunning = true;
        this.eventManager.TriggerEvent(GameControllerEvents.START_DAYTIME);
        OnDayTransition?.Invoke(true);
    }

    public void TriggerDayTimeStop() {
        this.time = this.BEGINNING_MAX_COUNTDOWN;
        this.isTimerRunning = true;
        this.isTransitionCountdown = true;
        this.eventManager.TriggerEvent(GameControllerEvents.STOP_DAYTIME);
    }

    public void TriggerNightTimeStart() {
        this.isTransitionCountdown = false;
        this.isStartCountdown = false;
        this.isDayTime = false;

        this.time = this.GetCurrCollectedNightTimeAmount();
        this.isTimerRunning = true;
        this.eventManager.TriggerEvent(GameControllerEvents.START_NIGHTTIME);
        OnDayTransition?.Invoke(false);
    }

    public void TriggerNightTimeStop() {
        AddScore(this.currCollectedNightTime);
        this.currCollectedNightTime = 0;

        this.time = this.BEGINNING_MAX_COUNTDOWN;

        this.isTimerRunning = true;
        this.isTransitionCountdown = true;

        this.eventManager.TriggerEvent(GameControllerEvents.STOP_NIGHTTIME);
    }

    public float GetCurrCollectedNightTimeAmount () {
        float minAmountToStartNightWith = 3.0f; // seconds
        return Mathf.Max(minAmountToStartNightWith, Mathf.Min(this.DAY_TIME_MAX_COUNTDOWN, this.currCollectedNightTime * this.AMOUNT_OF_TIME_PER_COLLECTED_NIGHTTIME));
    }

    public float GetCurrCollectedNightTimePercent () {
        return this.GetCurrCollectedNightTimeAmount() / this.DAY_TIME_MAX_COUNTDOWN;
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
