using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public ObjectPool Pool { get; set; }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void ReturnToPool()
    {
        if (!Pool)
        {
            Debug.LogError(gameObject.name + " is not connected to a pool.");
            return;
        }

        Pool.ReturnToPool(this);
    }


}
