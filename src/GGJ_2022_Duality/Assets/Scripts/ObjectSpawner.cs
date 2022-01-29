using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private PoolManager pm => PoolManager.Instance;
    private LevelChunk levelChunk;

    public PoolObject[] gameObjectsToSpawn;

    private PoolObject spawnedObject;

    protected void Awake()
    {
        levelChunk = GetComponentInParent<LevelChunk>();
    }

    protected void OnEnable()
    {
        levelChunk.OnExitChunk += ReturnSpawnedObjectToPool;
    }

    protected void OnDisable()
    {
        levelChunk.OnExitChunk -= ReturnSpawnedObjectToPool;
    }

    public void SpawnObject()
    {
        int newIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, gameObjectsToSpawn.Length - 1));
        ObjectPool p = pm.GetPool(gameObjectsToSpawn[newIndex].name);

        spawnedObject = p.Get();
        spawnedObject.transform.SetParent(this.transform);
        spawnedObject.transform.localPosition = Vector3.zero;
    }

    private void ReturnSpawnedObjectToPool(int id)
    {
        if (spawnedObject == null) { return; }

        if (spawnedObject.gameObject.activeInHierarchy)
        {
            spawnedObject.ReturnToPool();
        }

        spawnedObject = null;
    }
}
