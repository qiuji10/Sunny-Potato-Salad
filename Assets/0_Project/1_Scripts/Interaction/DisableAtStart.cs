using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAtStart : MonoBehaviour
{
    private void Start()
    {
        Invoke("DisableObject", 1f);   
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
