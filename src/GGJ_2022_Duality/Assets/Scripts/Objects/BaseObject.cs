using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BaseObject : MonoBehaviour
{
    private GameController gameController;

    protected Collider2D col;

    public bool isDaytimeNegative = true;

    private bool isDualAction;

    public UnityAction isNightTimeEvent = delegate { };
    public UnityAction isDayTimeEvent = delegate { };

    private PositiveObjectAction poa;
    private NegativeObjectAction noa;

    private SpriteRenderer[] allSR;
    protected virtual void Awake()
    {
        allSR = GetComponentsInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        gameController = FindObjectOfType<GameController>();
        gameController.StartListening(GameControllerEvents.START_NIGHTTIME, isNightTimeEvent);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, isDayTimeEvent);

        poa = GetComponent<PositiveObjectAction>();
        noa = GetComponent<NegativeObjectAction>();
    }

    protected virtual void Start()
    {
        if (poa && noa)
        {
            Debug.Log("Dual Action Set!");
            isDualAction = true;
            SetUpActions(true);
        }
    }

    protected void OnEnable()
    {
        isNightTimeEvent += OnNightTime;
        isDayTimeEvent += OnDayTime;
    }

    private void SetUpActions(bool isDaytime)
    {
        if (!isDualAction) { return; }

        if (isDaytime)
        {
            if (isDaytimeNegative)
            {
                poa.canAction = false;
                noa.canAction = true;
            }
            else
            {
                poa.canAction = true;
                noa.canAction = false;
            }
        }
        else
        {
            if (isDaytimeNegative)
            {
                poa.canAction = true;
                noa.canAction = false;
                SetAllSRColor(Color.green);
            }
            else
            {
                SetAllSRColor(Color.red);
                poa.canAction = false;
                noa.canAction = true;
            }
        }
    }

    private void SetAllSRColor(Color color)
    {
        for(int i = 0; i < allSR.Length; i++)
        {
            allSR[i].color = color;
        }
    }

    private void OnDayTime()
    {
        SetUpActions(true);
    }

    private void OnNightTime()
    {
        SetUpActions(false);
    }

    protected void OnDisable()
    {
        isNightTimeEvent -= OnNightTime;
        isDayTimeEvent -= OnDayTime;
    }



}
