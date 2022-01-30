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
    private HealObjectSpawner[] healSpawners;
    private HurtObjectSpawner[] hurtSpawners;

    public Action<bool> OnChunkSpawn = delegate { };
    public Action<int> OnEnterChunk = delegate { };
    public Action<int> OnMiddleChunk = delegate { };
    public Action<int> OnExitChunk = delegate { };

    public int chunkID;

    public int chunkLength = 100;
    public int chunkHeight = 50;

    private SpriteRenderer background;

    private GameController gc => GameController.Instance;

    private void Awake()
    {
        background = GetComponentInChildren<SpriteRenderer>();
        chunkStartPos = GetComponentInChildren<ChunkStartPosition>();
        chunkMidPos = GetComponentInChildren<ChunkMidPosition>();
        chunkEndPos = GetComponentInChildren<ChunkEndPosition>();
        pool = GetComponent<PoolObject>();

        tilemaps = GetComponentsInChildren<Tilemap>();
        healSpawners = GetComponentsInChildren<HealObjectSpawner>();
        hurtSpawners = GetComponentsInChildren<HurtObjectSpawner>();
        gc.OnDayTransition += OnDayTransition;
    }

    private void OnDayTransition(bool isDay)
    {
        if (isDay)
        {
            background.color = new Color(0.8f, 0.8f, 0.8f);
        }
        else
        {
            background.color = new Color(0.2f, 0.2f, 0.2f);
        }
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

        OnChunkSpawn?.Invoke(true);

        SpawnHealObjects();
        SpawnHurtObjects();      
    }

    private void SpawnHealObjects()
    {
        int spawnersToSpawn = healSpawners.Length / 2;
        List<int> indexesSelected = new List<int>();
        for (int i = 0; i < spawnersToSpawn; i++)
        {
            int newIndex;
            do
            {
                newIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, healSpawners.Length - 1));
            } while (indexesSelected.Contains(newIndex));
            indexesSelected.Add(newIndex);
        }

        foreach (int i in indexesSelected)
        {
            healSpawners[i].SpawnObject();
        }
    }

    private void SpawnHurtObjects()
    {
        int spawnersToSpawn = hurtSpawners.Length / 2;
        List<int> indexesSelected = new List<int>();
        for (int i = 0; i < spawnersToSpawn; i++)
        {
            int newIndex;
            do
            {
                newIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, hurtSpawners.Length - 1));
            } while (indexesSelected.Contains(newIndex));
            indexesSelected.Add(newIndex);
        }

        foreach (int i in indexesSelected)
        {
            hurtSpawners[i].SpawnObject();
        }
    }

    public void Despawn()
    {
        OnChunkSpawn?.Invoke(false);
        pool.DelayReturnToPool(1f);
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
    }

    public void PoolReset()
    {
        chunkID = -1;
        chunkStartPos.Reset();
        chunkMidPos.Reset();
        chunkEndPos.Reset();
    }
}
