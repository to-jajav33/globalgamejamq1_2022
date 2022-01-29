using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public string prefabLevelChunkName = "Chunk-";

    private int totalChunkNum = 0;

    private LevelChunk activeChunk;
    private PlayerController pController;

    private Queue<LevelChunk> spawnedChunkQueue = new Queue<LevelChunk>();

    private PoolManager pManager;
    private ObjectPool startChunkPool;
    public int numDiffChunks = 4;
    private ObjectPool[] chunksPools;

    private void Awake()
    {
        pController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        pManager = PoolManager.Instance;
        startChunkPool = pManager.GetPool(prefabLevelChunkName + 'S');

        chunksPools = new ObjectPool[numDiffChunks];
        for(int i = 0; i < chunksPools.Length; i++)
        {
            chunksPools[i] = pManager.GetPool(prefabLevelChunkName + i.ToString());
        }

        SpawnChunk();
    }

    private void SpawnChunk()
    {
        LevelChunk spawnedChunk = GetChunkfromPool();
        spawnedChunk.transform.parent = this.transform;
        spawnedChunkQueue.Enqueue(spawnedChunk);

        if (totalChunkNum == 0)
        {
            SetActiveChunk();
            activeChunk.transform.position = pController.transform.position - new Vector3(activeChunk.chunkLength / 2f, 0,0);
        }
        else
        {
            spawnedChunk.transform.position = activeChunk.ChunkEndPos.transform.position;
        }

        spawnedChunk.SetUpChunk(totalChunkNum);
        totalChunkNum++;
    }

    private LevelChunk GetChunkfromPool()
    {
        if (totalChunkNum == 0)
        {
            return startChunkPool.Get().GetComponent<LevelChunk>();
        }

        int randPool = UnityEngine.Random.Range(0, chunksPools.Length - 1);
        return chunksPools[randPool].Get().GetComponent<LevelChunk>();
    }

    private void SetActiveChunk()
    {
        if (activeChunk)
        {
            activeChunk.OnEnterChunk -= EnterChunk;
            activeChunk.OnMiddleChunk -= MiddleChunk;
            activeChunk.OnExitChunk -= ExitChunk;
        }

        activeChunk = spawnedChunkQueue.Dequeue();
        activeChunk.OnEnterChunk += EnterChunk;
        activeChunk.OnMiddleChunk += MiddleChunk;
        activeChunk.OnExitChunk += ExitChunk;
    }

    private void EnterChunk(int ID)
    {

    }

    private void MiddleChunk(int ID)
    {
        SpawnChunk();
    }

    private void ExitChunk(int ID)
    {
        SetActiveChunk();
    }
}
