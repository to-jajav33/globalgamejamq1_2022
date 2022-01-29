using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(PoolObject))]
public class HitObject : BaseObject
{
    public Action IsHit = delegate { };


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Object hit");

            IsHit?.Invoke();
        }
    }
}
