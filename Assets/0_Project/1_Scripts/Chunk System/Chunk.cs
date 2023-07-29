using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int chunkSize;
    public float spacing = 1.0f;
    public Ground groundPrefab;
    public Vector3 groundOffset = new Vector3(0, -1.5f, 0);

    [Header("References")]
    public Transform chunkContainer;
    public Transform obstacleContainer;

    private Coroutine chunkTask;
    private BoxCollider _collider;
    private List<Ground> _childs = new List<Ground>();
    private List<Vector2Int> _worldObjects = new List<Vector2Int>();

    public int maxTile => chunkSize.x * chunkSize.y;

    public List<Vector2Int> WorldObjects => _worldObjects;

    public static event Action<Chunk> OnEnterChunk;

    private void Start()
    {
        _worldObjects.Add(new Vector2Int(1, 1));

        SetupChunk();
        _collider = GetComponent<BoxCollider>();
        UpdateColliderSize();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnEnterChunk?.Invoke(this);
        }
    }

    [Button]
    public void SetupChunk()
    {
        float xOffset = chunkSize.x % 2 == 0 ? 0.5f : 0f;
        float yOffset = chunkSize.y % 2 == 0 ? 0.5f : 0f;

        for (int x = -chunkSize.x / 2; x < chunkSize.x / 2; x++)
        {
            for (int y = -chunkSize.y / 2; y < chunkSize.y / 2; y++)
            {
                Vector3 groundPosition = new Vector3((x + xOffset) * spacing, 0, (y + yOffset) * spacing);
                Ground newGround = Instantiate(groundPrefab, chunkContainer);
                newGround.transform.localPosition = groundPosition + groundOffset;
                newGround.SetGroundState(GroundState.Default);
                _childs.Add(newGround);
            }
        }
    }

    public void ToggleChunk(bool isOn)
    {
        if (chunkTask != null)
            StopCoroutine(chunkTask);

        chunkTask = StartCoroutine(ToggleChunk_Task(isOn));
    }

    private IEnumerator ToggleChunk_Task(bool isOn)
    {
        if (!isOn)
            yield return new WaitForSeconds(2f);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isOn);
        }
    }

    public Vector2Int GetChunkPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);

        return new Vector2Int(x, z);
    }

    public Vector2Int GetChunkLocalPosition()
    {
        int x = Mathf.RoundToInt(transform.localPosition.x);
        int z = Mathf.RoundToInt(transform.localPosition.z);

        return new Vector2Int(x, z);
    }

    //public Vector2Int GetRandomPosition()
    //{
    //    Vector2Int chunkPos = GetChunkPosition();

    //    int minX = chunkPos.x - chunkSize.x -1 / 2;
    //    int maxX = chunkPos.x + chunkSize.x + 1 / 2;
    //    int minY = chunkPos.y - chunkSize.y - 1 / 2;
    //    int maxY = chunkPos.y + chunkSize.y + 1 / 2;

    //    int randomX = Random.Range(minX, maxX + 2);
    //    int randomY = Random.Range(minY, maxY + 2);

    //    return new Vector2Int(randomX, randomY);
    //}

    public bool AddObjectPosition(Vector2Int position)
    {
        if (_worldObjects.Count >= maxTile || _worldObjects.Contains(position))
        {
            return false;
        }
        else
        {
            _worldObjects.Add(position);
            return true;
        }
    }

    public Ground GetNearestGround(Vector3 position)
    {
        Ground nearestGround = null;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < _childs.Count; i++)
        {
            Ground currentGround = _childs[i];
            Vector3 groundPosition = currentGround.transform.position;
            float distance = Vector3.Distance(position, groundPosition);

            if (distance < nearestDistance)
            {
                nearestGround = currentGround;
                nearestDistance = distance;
            }
        }

        return nearestGround;
    }

    private void UpdateColliderSize()
    {
        float totalChunkWidth = chunkSize.x * spacing;
        float totalChunkHeight = chunkSize.y * spacing;
        _collider.size = new Vector3(totalChunkWidth, 2f, totalChunkHeight);
    }

    [Button]
    private void ClearChunk()
    {
        List<GameObject> childrenToDestroy = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childrenToDestroy.Add(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < childrenToDestroy.Count; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(childrenToDestroy[i]);
#else
        Destroy(childrenToDestroy[i]);
#endif
        }
    }
}
