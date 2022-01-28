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

    public UnityAction isNightTimeEvent = delegate { };
    public UnityAction isDayTimeEvent = delegate { };


    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        gameController = FindObjectOfType<GameController>();
        gameController.StartListening(GameControllerEvents.START_NIGHTTIME, isNightTimeEvent);
        gameController.StartListening(GameControllerEvents.START_DAYTIME, isDayTimeEvent);
    }

}
