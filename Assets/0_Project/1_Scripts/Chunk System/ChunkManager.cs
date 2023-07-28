using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] Chunk chunkPrefab;

    private List<Chunk> chunks = new List<Chunk>();
    private Dictionary<Vector2Int, Chunk> enabledChunks = new Dictionary<Vector2Int, Chunk>();

    private Chunk activeChunk;

    private void Awake()
    {
        Chunk.OnEnterChunk += Chunk_OnEnterChunk;

        Chunk[] chunks = FindObjectsOfType<Chunk>();

        foreach (Chunk chunk in chunks)
        {
            this.chunks.Add(chunk);
            enabledChunks.Add(chunk.GetChunkPosition(), chunk);
        }
    }

    private void OnDestroy()
    {
        Chunk.OnEnterChunk -= Chunk_OnEnterChunk;
    }

    private void Chunk_OnEnterChunk(Chunk chunk)
    {
        activeChunk = chunk;
        RecalculateChunk();
    }

    private void RecalculateChunk()
    {
        if (activeChunk == null)
        {
            Debug.LogWarning("No active chunk found!");
            return;
        }

        Vector2Int activeChunkPos = activeChunk.GetChunkPosition();

        int chunkOffset = activeChunk.chunkSize.x + activeChunk.chunkSize.y;

        List<Vector2Int> directions = new List<Vector2Int>()
        {
            new Vector2Int(0, chunkOffset),
            new Vector2Int(0, -chunkOffset),
            new Vector2Int(-chunkOffset, 0),
            new Vector2Int(chunkOffset, 0),

            new Vector2Int(chunkOffset, chunkOffset),
            new Vector2Int(chunkOffset, -chunkOffset),
            new Vector2Int(-chunkOffset, chunkOffset),
            new Vector2Int(-chunkOffset, -chunkOffset),
        };

        List<Vector2Int> neighbourPosition = new List<Vector2Int>();

        for (int i = 0; i < directions.Count; i++)
        {
            Vector2Int neighborChunkPos = activeChunkPos + directions[i];

            neighbourPosition.Add(neighborChunkPos);

            if (!enabledChunks.ContainsKey(neighborChunkPos))
            {
                Chunk chunk = chunks.Find((chunkData) => chunkData.GetChunkPosition() == neighborChunkPos);

                if (chunk == null)
                {
                    Chunk newChunk = Instantiate(chunkPrefab);
                    newChunk.transform.position = new Vector3(neighborChunkPos.x, activeChunk.transform.position.y, neighborChunkPos.y);

                    chunks.Add(newChunk);
                    enabledChunks.Add(neighborChunkPos, newChunk);

                }
                else
                {
                    chunk.EnableChunk();
                    enabledChunks.Add(neighborChunkPos, chunk);
                }
                
            }
        }

        // Find chunks to disable (those that are not within the 3x3 range of the active chunk)
        List<Vector2Int> chunksToDisable = new List<Vector2Int>();
        foreach (var item in enabledChunks)
        {
            Vector2Int chunkPos = item.Key;

            if (!neighbourPosition.Contains(chunkPos))
            {
                chunksToDisable.Add(chunkPos);
            }
        }

        // Disable the chunks that are no longer within the 3x3 range
        foreach (var chunkPos in chunksToDisable)
        {
            if (enabledChunks[chunkPos] == activeChunk)
                continue;

            enabledChunks[chunkPos].DisableChunk();
            enabledChunks.Remove(chunkPos);
        }

        //foreach (var item in enabledChunks)
        //{
        //    if (item.Value == activeChunk)
        //        continue;

        //    if (!neighbourPosition.Contains(item.Key))
        //    {
        //        item.Value.DisableChunk();
        //    }
        //}
    }
}
