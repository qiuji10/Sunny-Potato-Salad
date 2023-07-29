using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundState { Default, Digged }

public class Ground : MonoBehaviour
{
    private GroundState _state;
    public GroundState State => _state;

    private MeshRenderer _mesh;

    private void Awake()
    {
        _mesh = GetComponent<MeshRenderer>();
    }

    public void SetGroundState(GroundState state)
    {
        MaterialManager manager = MaterialManager.Instance;

        this._state = state;

        switch (state)
        {
            case GroundState.Default:
                _mesh.material = manager.GetMaterial("default_ground");
                break;

            case GroundState.Digged:
                _mesh.material = manager.GetMaterial("digged_ground");
                break;
        }
    }
}
