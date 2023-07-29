using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundState { Default, Digged }

public class Ground : MonoBehaviour
{
    private GroundState _state;
    public GroundState State => _state;

    private MeshRenderer _mesh;

    private string[] defaultGround = { "default_ground_1", "default_ground_2", "default_ground_3" };
    private string[] dirtGround = { "dirt_ground_1", "dirt_ground_2", "dirt_ground_3" };

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();

        transform.rotation = Quaternion.Euler(-90f, 0f, Random.Range(0, 4) * 90f);
        SetGroundState((GroundState)Random.Range(0, 2));
    }

    public void SetGroundState(GroundState state)
    {
        this._state = state;

        MaterialManager manager = MaterialManager.Instance;

        switch (state)
        {
            case GroundState.Default:
                _mesh.materials = GetMaterials(RandomString(defaultGround));
               //_mesh.material = manager.GetBaseMaterial("default_ground");
                break;

            case GroundState.Digged:
                _mesh.materials = GetMaterials(RandomString(dirtGround));
                //_mesh.material = manager.GetBaseMaterial("dirt_ground");
                break;
        }
    }

    private Material[] GetMaterials(string name)
    {
        MaterialManager manager = MaterialManager.Instance;

        Material[] materials = new Material[2];
        materials[0] = manager.GetBaseMaterial(name);
        materials[1] = manager.GetExtraMaterial(name);

        return materials;
    }

    private string RandomString(string[] list)
    {
        return list[Random.Range(0, list.Length)];
    }
}
