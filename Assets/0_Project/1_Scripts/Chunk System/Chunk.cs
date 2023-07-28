using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector2Int chunkSize;
    public Ground groundPrefab;

    [Button]
    public void SetupChunk()
    {
        ClearChunk();

        for (int x = -chunkSize.x / 2; x < chunkSize.x / 2; x++)
        {
            for (int y = -chunkSize.y / 2; y < chunkSize.y / 2; y++)
            {
                Vector3 groundPosition = new Vector3(x, 0, y);
                Ground newGround = Instantiate(groundPrefab, transform);
                newGround.transform.localPosition = groundPosition;
                newGround.SetGroundState(GroundState.Default);
            }
        }
    }

    [Button]
    public void ClearChunk()
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
