using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerEvent : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    void Awake()
    {
        Timer.OnTimerChange += Timer_OnTimerChange;
    }

    void OnDestroy()
    {
        Timer.OnTimerChange -= Timer_OnTimerChange;
    }

    private void Timer_OnTimerChange(float time)
    {
        if (timerText) timerText.text = Timer.FormatTime(time);
    }
}
