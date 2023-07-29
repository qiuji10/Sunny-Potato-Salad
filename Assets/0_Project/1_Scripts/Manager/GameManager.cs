using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Timer timer;

    private void Awake()
    {
        Timer.OnTimerStop += Timer_OnTimerStop;
    }

    private void OnDestroy()
    {
        Timer.OnTimerStop -= Timer_OnTimerStop;
    }

    private void Timer_OnTimerStop()
    {
        
    }

    private void Start()
    {
        timer.StartTimer();
    }
}
