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

        transform.GetChild(0).rotation = Quaternion.Euler(-90f, 0f, Random.Range(0, 4) * 90f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //_collider.enabled = false;
            StartCoroutine(TemporarilyDeactive(0.15f));
            OnPlayerCollide?.Invoke();
        }

        if (col.CompareTag("Shield"))
        {
            StartCoroutine(TemporarilyDeactive(0.15f));
        }
    }

    private IEnumerator TemporarilyDeactive(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
