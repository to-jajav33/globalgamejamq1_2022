using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PoolObject))]
public class PickUpObject : BaseObject
{
    public Action IsTriggered = delegate { };
    protected PoolObject poolObject;

    protected override void Awake()
    {
        base.Awake();
        poolObject = GetComponent<PoolObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsTriggered?.Invoke();
            poolObject.ReturnToPool();
        }
    }
}
