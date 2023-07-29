using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MaterialData
{
    public string materialName;
    public Material baseMaterial;
    public Material extraMaterial;
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

    public Material GetBaseMaterial(string materialName)
    {
        return materials.Find((data) => data.materialName == materialName).baseMaterial;
    }

    public Material GetExtraMaterial(string materialName)
    {
        return materials.Find((data) => data.materialName == materialName).extraMaterial;
    }
}
