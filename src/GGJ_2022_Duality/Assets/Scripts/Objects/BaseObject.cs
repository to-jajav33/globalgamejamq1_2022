using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class BaseObject : MonoBehaviour
{
    private GameController gameController => GameController.Instance;

    protected Collider2D col;

    public bool isDaytimeNegative = true;

    private bool isDualAction;

    public UnityAction isNightTimeEvent = delegate { };
    public UnityAction isDayTimeEvent = delegate { };

    private PositiveObjectAction poa;
    private NegativeObjectAction noa;

    private SpriteRenderer[] allSR;

    private Material helpMaterial;
    private Material hurtMaterial;
    protected virtual void Awake()
    {
        allSR = GetComponentsInChildren<SpriteRenderer>();
        col = GetComponent<Collider2D>();



        poa = GetComponent<PositiveObjectAction>();
        noa = GetComponent<NegativeObjectAction>();

        helpMaterial = Resources.Load<Material>("Outline-Help");
        hurtMaterial = Resources.Load<Material>("Outline-Hurt");
    }

    protected virtual void Start()
    {
        if (poa && noa)
        {
            isDualAction = true;
            SetUpActions(true);
        }
        else
        {
            if (poa)
            {
                SetAllSRMaterial(helpMaterial);
            }

            if (noa)
            {
                SetAllSRMaterial(hurtMaterial);
            }
        }

    }

    protected void OnEnable()
    {
        gameController.OnDayTransition += OnDayTime;
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
                SetAllSRMaterial(hurtMaterial);
            }
            else
            {
                poa.canAction = true;
                noa.canAction = false;
                SetAllSRMaterial(helpMaterial);
            }
        }
        else
        {
            if (isDaytimeNegative)
            {
                poa.canAction = true;
                noa.canAction = false;
                SetAllSRMaterial(helpMaterial);
            }
            else
            {
                SetAllSRMaterial(hurtMaterial);
                poa.canAction = false;
                noa.canAction = true;
            }
        }
    }

    private void SetAllSRMaterial(Material mat)
    {
        for(int i = 0; i < allSR.Length; i++)
        {
            allSR[i].material = mat;
        }
    }

    private void OnDayTime(bool isDaytime)
    {
        SetUpActions(isDaytime);
    }

    protected void OnDisable()
    {
        if(gameController)
            gameController.OnDayTransition -= OnDayTime;
    }



}
