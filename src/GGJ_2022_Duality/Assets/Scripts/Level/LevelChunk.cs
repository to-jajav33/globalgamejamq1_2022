using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class LevelChunk : MonoBehaviour, IPoolReset
{
    private ChunkStartPosition chunkStartPos;

    private ChunkEndPosition chunkEndPos;
    public ChunkEndPosition ChunkEndPos { get { return chunkEndPos; } }

    private ChunkMidPosition chunkMidPos;
    private PoolObject pool;

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
    }

    public void SetUpChunk(int _chunkID)
    {
        chunkID = _chunkID;
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
