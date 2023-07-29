using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private PlayerController playerController;

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
        countdownText.text = "TIME OVER";
        playerController.cantMove = true;
    }

    private IEnumerator Start()
    {
        playerController.cantMove = true;
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO !";
        playerController.cantMove = false;
        timer.StartTimer();

        yield return new WaitForSeconds(1f);
        countdownText.text = "";
    }
}
