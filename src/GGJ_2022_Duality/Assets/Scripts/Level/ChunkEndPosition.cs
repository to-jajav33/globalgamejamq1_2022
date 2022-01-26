using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkEndPosition : MonoBehaviour
{
    private LevelChunk parentChunk;

    private bool isPlayerTriggered = false;
    private void Awake()
    {
        parentChunk = GetComponentInParent<LevelChunk>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isPlayerTriggered)
        {
            isPlayerTriggered = true;
            parentChunk.PlayerExitedChunk();
        }
    }
}
