using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour
{
    [SerializeField] bool randomRotation = true;

    private int Break;
    private Animator _anim;
    
    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();

        Break = Animator.StringToHash("Break");

        if (randomRotation)
            transform.GetChild(0).rotation = Quaternion.Euler(-90f, 0f, Random.Range(0, 4) * 90f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Shield"))
        {
            StartCoroutine(TemporarilyDeactive(0.45f));

            if (_anim != null)
                _anim.SetTrigger(Break);
        }
    }

    private IEnumerator TemporarilyDeactive(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
