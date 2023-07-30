using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tree") || col.CompareTag("Rock"))
        {
            StartCoroutine(TemporarilyDeactive(0.15f));
        }
    }

    private IEnumerator TemporarilyDeactive(float duration)
    {
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }
}
