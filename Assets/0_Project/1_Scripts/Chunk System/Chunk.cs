using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int chunkSize;
    public float spacing = 1.0f;
    public Ground groundPrefab;

    private BoxCollider _collider;
    private List<GameObject> childs = new List<GameObject>();

    public static event Action<Chunk> OnEnterChunk;

    private void Start()
    {
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
                Ground newGround = Instantiate(groundPrefab, transform);
                newGround.transform.localPosition = groundPosition;
                newGround.SetGroundState(GroundState.Default);
                childs.Add(newGround.gameObject);
            }
        }
    }

    public void EnableChunk()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void DisableChunk()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public Vector2Int GetChunkPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);

        return new Vector2Int(x, z);
    }

    private void UpdateColliderSize()
    {
        // Calculate the total size of the chunk including spacing
        float totalChunkWidth = chunkSize.x * spacing;
        float totalChunkHeight = chunkSize.y * spacing;
        _collider.size = new Vector3(totalChunkWidth, 2f, totalChunkHeight);
    }

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
