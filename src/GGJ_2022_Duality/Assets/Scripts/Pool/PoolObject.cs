using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public ObjectPool Pool { get; set; }

    private IPoolReset[] resetObjects;

    private void Awake()
    {
        resetObjects = GetComponentsInChildren<IPoolReset>();
    }

    private void Start()
    {
        if (Pool == null)
        {
            Pool = PoolManager.Instance.GetPool(gameObject.name);
        }
    }
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

        foreach (IPoolReset pr in resetObjects)
        {
            pr.PoolReset();
        }

        Pool.ReturnToPool(this);
    }

    public void DelayReturnToPool(float delay)
    {
        StartCoroutine(coDelayReturnToPool(delay));
    }

    private IEnumerator coDelayReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }


}
