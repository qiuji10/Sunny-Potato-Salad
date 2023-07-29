using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private void Start()
    {
        GameManager.treasureChests.Add(transform);
    }

    private void OnDestroy()
    {
        GameManager.treasureChests.Remove(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.treasureChests.Remove(transform);
        }
    }
}
