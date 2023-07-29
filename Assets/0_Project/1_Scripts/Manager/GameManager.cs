using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Timer timer;

    private void Start()
    {
        timer.StartTimer();
    }
}
