using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolObject))]
public class LevelChunk : MonoBehaviour
{
    private ChunkStartPosition chunkStartPos;
    private ChunkEndPosition chunkEndPos;
    private PoolObject pool;

    private void Awake()
    {
        chunkStartPos = GetComponentInChildren<ChunkStartPosition>();
        chunkEndPos = GetComponentInChildren<ChunkEndPosition>();
    }

    public void PlayerEnteredChunk()
    {
        Debug.Log("PlayerEnteredChunk");
    }

    public void PlayerExitedChunk()
    {
        Debug.Log("PlayerExitedChunk");
    }
}
