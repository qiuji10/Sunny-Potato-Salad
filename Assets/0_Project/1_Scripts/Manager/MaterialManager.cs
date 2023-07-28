using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MaterialData
{
    public string materialName;
    public Material material;
}

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;

    [SerializeField] private List<MaterialData> materials = new List<MaterialData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Material GetMaterial(string materialName)
    {
        return materials.Find((data) => data.materialName == materialName).material;
    }
}
