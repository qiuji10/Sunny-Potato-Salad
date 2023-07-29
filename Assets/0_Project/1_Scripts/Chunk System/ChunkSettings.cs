using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public string objectName;
    public GameObject prefab;
    public Vector2Int minMaxRange = new Vector2Int(0, 1);
    public AnimationCurve distribution;
}

[CreateAssetMenu(fileName = "Chunk Settings", menuName = "Menu/Chunk Settings")]
public class ChunkSettings : ScriptableObject
{
    public List<ObjectData> worldObjects = new List<ObjectData>();

    public GameObject GetPrefab(string objectName)
    {
        return GetObject(objectName).prefab;
    }

    public int GetRandomAmount(string objectName)
    {
        ObjectData data = GetObject(objectName);

        AnimationCurveSampler sampler = new AnimationCurveSampler(data.distribution);

        return Mathf.RoundToInt(Mathf.Lerp(data.minMaxRange.x, data.minMaxRange.y + 1, sampler.RandomSample()));
    }

    private ObjectData GetObject(string objectName)
    {
        return worldObjects.Find((objectData) => objectData.objectName.ToLower() == objectName.ToLower());
    }
}
