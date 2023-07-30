using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Chunk chunkPrefab;
    [SerializeField] ChunkSettings chunkSettings;

    private List<Chunk> _chunks = new List<Chunk>();
    private Dictionary<Vector2Int, Chunk> _enabledChunks = new Dictionary<Vector2Int, Chunk>();

    private static Chunk activeChunk;

    private void Awake()
    {
        activeChunk = null;

        Chunk.OnEnterChunk += Chunk_OnEnterChunk;

        Chunk[] chunks = FindObjectsOfType<Chunk>();

        if (chunks.Length > 0)
        {
            foreach (Chunk chunk in chunks)
            {
                this._chunks.Add(chunk);
                _enabledChunks.Add(chunk.GetChunkPosition(), chunk);
            }
        }
        else
        {
            Chunk chunk = Instantiate(chunkPrefab);

            _chunks.Add(chunk);
            _enabledChunks.Add(Vector2Int.zero, chunk);

            SpawnObject(chunk);
            Chunk_OnEnterChunk(chunk);
        }
    }

    private void OnDestroy()
    {
        Chunk.OnEnterChunk -= Chunk_OnEnterChunk;
    }

    private void Chunk_OnEnterChunk(Chunk chunk)
    {
        if (activeChunk == chunk)
            return;

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

            if (!_enabledChunks.ContainsKey(neighborChunkPos))
            {
                Chunk chunk = _chunks.Find((chunkData) => chunkData.GetChunkPosition() == neighborChunkPos);

                if (chunk == null)
                {
                    Chunk newChunk = Instantiate(chunkPrefab);
                    newChunk.transform.position = new Vector3(neighborChunkPos.x, activeChunk.transform.position.y, neighborChunkPos.y);

                    _chunks.Add(newChunk);
                    _enabledChunks.Add(neighborChunkPos, newChunk);

                    SpawnObject(newChunk);
                }
                else
                {
                    chunk.gameObject.SetActive(true);
                    chunk.ToggleChunk(true);
                    _enabledChunks.Add(neighborChunkPos, chunk);
                }
                
            }
        }

        List<Vector2Int> chunksToDisable = new List<Vector2Int>();
        foreach (var item in _enabledChunks)
        {
            Vector2Int chunkPos = item.Key;

            if (!neighbourPosition.Contains(chunkPos))
            {
                chunksToDisable.Add(chunkPos);
            }
        }

        foreach (var chunkPos in chunksToDisable)
        {
            if (_enabledChunks[chunkPos] == activeChunk)
                continue;

            _enabledChunks[chunkPos].ToggleChunk(false);
            _enabledChunks[chunkPos].gameObject.SetActive(false);
            _enabledChunks.Remove(chunkPos);
        }
    }

    private void SpawnObject(Chunk chunk)
    {
        SpawnObject("Tree", chunk);
        SpawnObject("Stone1", chunk);
        SpawnObject("Stone2", chunk);
        SpawnObject("TreasureChest", chunk);
    }

    private void SpawnObject(string objectName, Chunk chunk)
    {
        GameObject objectPrefab = chunkSettings.GetPrefab(objectName);
        int randomAmount = chunkSettings.GetRandomAmount(objectName);

        if (randomAmount > chunk.maxTile)
        {
            Debug.Log("Settings Overflow");
            return;
        }

        for (int j = 0; j < randomAmount; j++)
        {
            Vector2Int randPos = GenerateRandomPosition(chunk);

            if (randPos == Vector2Int.one)
                continue;

            if (chunk.AddObjectPosition(randPos))
            {
                GameObject worldObject = Instantiate(objectPrefab, chunk.obstacleContainer);
                worldObject.transform.position = new Vector3(randPos.x, 1, randPos.y);
            }
        }
    }

    private Vector2Int GenerateRandomPosition(Chunk chunk)
    {
        Vector2Int position = chunk.GetChunkLocalPosition() + new Vector2Int(Random.Range(-chunk.chunkSize.x, chunk.chunkSize.x), Random.Range(-chunk.chunkSize.x, chunk.chunkSize.x));

        if (position.x % 2 == 0)
        {
            if (position.x == 6)
            {
                position.x = 7;
            }
            else
            {
                position.x -= 1; // Make the x coordinate odd
            }
        }

        if (position.y % 2 == 0)
        {
            

            if (position.y == 6)
            {
                position.y = 7;
            }
            else
            {
                position.y -= 1; // Make the y coordinate odd
            }
        }

        return position;
    }

    public static Ground GetGround(Vector3 position)
    {
        return activeChunk.GetNearestGround(position);
    }
}
