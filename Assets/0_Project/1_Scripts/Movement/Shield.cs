using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Collider _collider;
    public MeshRenderer _meshRenderer;


    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Wood") || col.CompareTag("Rock"))
        {
            StartCoroutine(TemporarilyDeactive(0.15f));
        }
    }

    private IEnumerator TemporarilyDeactive(float duration)
    {
        yield return new WaitForSeconds(duration);
        _collider.enabled = false;
        _meshRenderer.enabled = false;
    }
}