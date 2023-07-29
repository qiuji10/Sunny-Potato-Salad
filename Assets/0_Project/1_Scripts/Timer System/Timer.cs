using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float countdownDuration = 60f;

    private float _remainingTime = 0f;
    private bool _isRunning = false;

    public static event Action<float> OnTimerChange;
    public static event Action OnTimerStop;

    [Button]
    public void StartTimer()
    {
        _remainingTime = countdownDuration;
        _isRunning = true;
    }

    public void StartTimer(float duration)
    {
        _remainingTime = duration;
        _isRunning = true;
    }

    public void IncreaseTime(float extraTime)
    {
        _remainingTime += extraTime;
    }

    public void ResumeTimer()
    {
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _remainingTime = countdownDuration;
        _isRunning = false;
    }

    public string GetTime()
    {
        return _remainingTime < 3f ? $"<color=red>{FormatTime(_remainingTime)}</color>" : FormatTime(_remainingTime);
    }

    private void Update()
    {
        if (_isRunning)
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime <= 0f)
            {
                _remainingTime = 0f;
                _isRunning = false;
                OnTimerStop?.Invoke();
            }

            OnTimerChange?.Invoke(_remainingTime);
            //Debug.Log("Time Left: " + GetTime());
        }
    }

    public static string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
