using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour
{
    [SerializeField] UnityEvent OnPlayerCollide;

    private Collider _collider;
    

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //_collider.enabled = false;
            StartCoroutine(TemporarilyDeactive(0.15f));
            OnPlayerCollide?.Invoke();
        }
    }

    private IEnumerator TemporarilyDeactive(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
