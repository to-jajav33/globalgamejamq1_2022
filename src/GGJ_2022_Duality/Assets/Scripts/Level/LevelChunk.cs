using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PoolObject))]
public class LevelChunk : MonoBehaviour, IPoolReset
{
    private ChunkStartPosition chunkStartPos;

    private ChunkEndPosition chunkEndPos;
    public ChunkEndPosition ChunkEndPos { get { return chunkEndPos; } }

    private ChunkMidPosition chunkMidPos;
    private PoolObject pool;

    private Tilemap[] tilemaps;
    private ObjectSpawner[] allSpawners;

    public Action OnChunkSpawn = delegate { };
    public Action<int> OnEnterChunk = delegate { };
    public Action<int> OnMiddleChunk = delegate { };
    public Action<int> OnExitChunk = delegate { };

    public int chunkID;

    public int chunkLength = 100;
    public int chunkHeight = 50;

    private void Awake()
    {
        chunkStartPos = GetComponentInChildren<ChunkStartPosition>();
        chunkMidPos = GetComponentInChildren<ChunkMidPosition>();
        chunkEndPos = GetComponentInChildren<ChunkEndPosition>();
        pool = GetComponent<PoolObject>();

        tilemaps = GetComponentsInChildren<Tilemap>();
        allSpawners = GetComponentsInChildren<ObjectSpawner>();
    }

    private void OnEnable()
    {
        foreach (Tilemap t in tilemaps)
        {
            t.RefreshAllTiles();
        }
    }

    public void SetUpChunk(int _chunkID)
    {
        chunkID = _chunkID;

        OnChunkSpawn?.Invoke();

        int spawnersToSpawn = allSpawners.Length / 2;
        List<int> indexesSelected = new List<int>();
        for (int i = 0; i < spawnersToSpawn; i++)
        {
            int newIndex;
            do
            {
                newIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, allSpawners.Length - 1));
            } while (indexesSelected.Contains(newIndex));
            indexesSelected.Add(newIndex);
        }

        foreach (int i in indexesSelected)
        {
            allSpawners[i].SpawnObject();
        }

       
    }

    public void PlayerEnteredChunk()
    {
        OnEnterChunk.Invoke(chunkID);
    }

    public void PlayerMiddleChunk()
    {
        OnMiddleChunk(chunkID);
    }

    public void PlayerExitedChunk()
    {
        OnExitChunk(chunkID);
        pool.DelayReturnToPool(2f);
    }

    public void PoolReset()
    {
        chunkID = -1;
        chunkStartPos.Reset();
        chunkMidPos.Reset();
        chunkEndPos.Reset();
    }
}
