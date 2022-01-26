using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Dictionary<string, ObjectPool> poolDict = new Dictionary<string, ObjectPool>();
    private ObjectPool[] pools;

    private static PoolManager instance;
    public static PoolManager Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<PoolManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(this); }
        else { instance = this; }

        pools = GetComponentsInChildren<ObjectPool>();

        foreach (ObjectPool pool in pools)
        {
            if (!poolDict.ContainsKey(pool.PrefabID))
            {
                poolDict.Add(pool.PrefabID, pool);
            }
        }
    }

    public ObjectPool GetPool(string prefabID)
    {
        if (poolDict.ContainsKey(prefabID))
        {
            return poolDict[prefabID];
        }
        else
        {
            Debug.LogError("Something is asking PoolManager for a " + prefabID + "when that pool gameobject isn't a pool.");
            return null;
        }
    }
}
