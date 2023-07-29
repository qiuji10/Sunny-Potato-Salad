using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions.Examples;

public class GameManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private Timer timeManager;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private CanvasGroup textBG;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private CanvasGroup pauseBG;

    private void Awake()
    {
        Timer.OnTimerStop += Timer_OnTimerStop;
    }

    private void OnDestroy()
    {
        Timer.OnTimerStop -= Timer_OnTimerStop;
    }

    public void PauseGame()
    {
        playerController.cantMove = true;
        timeManager.StopTimer();
        StartCoroutine(GamePause_AnimationTask(true));
    }

    public void ResumeGame()
    {
        playerController.cantMove = false;
        Time.timeScale = 1;
        timeManager.ResumeTimer();
        StartCoroutine(GamePause_AnimationTask(false));
    }

    private void Timer_OnTimerStop()
    {
        countdownText.text = "TIME OVER";
        playerController.cantMove = true;
    }

    private IEnumerator Start()
    {
        textBG.alpha = 1;
        pauseButton.gameObject.SetActive(false);
        playerController.cantMove = true;
        float oriFontSize = countdownText.fontSize;
        countdownText.fontSize = oriFontSize + 40;
        StartCoroutine(CountdownText_AnimationTask());
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        StartCoroutine(CountdownText_AnimationTask());
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        StartCoroutine(CountdownText_AnimationTask());
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        StartCoroutine(GameStart_AnimationTask());
        countdownText.text = "Go Goo!";
        pauseButton.gameObject.SetActive(true);
        playerController.cantMove = false;
        countdownText.fontSize = oriFontSize;
        timeManager.StartTimer();

        yield return new WaitForSeconds(1f);
        countdownText.text = "";
        
    }

    private IEnumerator CountdownText_AnimationTask()
    {
        Vector3 oriPosition = countdownText.rectTransform.anchoredPosition;
        Vector3 destination = oriPosition + new Vector3(0, 5f, 0);
        Vector3 currentPos = oriPosition + new Vector3(0, -25f, 0);

        float timer = 0;
        float maxTime = 0.3f;

        while (timer < maxTime)
        {
            timer += Time.deltaTime;

            float ratio = timer / maxTime;

            countdownText.alpha = ratio;
            countdownText.rectTransform.anchoredPosition = Vector3.Lerp(currentPos, destination, ratio);

            yield return null;
        }

        timer = 0;
        maxTime = 0.1f;
        
        while (timer < maxTime)
        {
            timer += Time.deltaTime;

            float ratio = timer / maxTime;

            countdownText.rectTransform.anchoredPosition = Vector3.Lerp(destination, oriPosition, ratio);

            yield return null;
        }
    }

    private IEnumerator GameStart_AnimationTask()
    {
        Vector3 scale = countdownText.transform.localScale;
        Vector3 targetScale = scale + new Vector3(4, 4, 4);

        float timer = 0;
        float maxTime = 0.3f;

        yield return new WaitForSeconds(0.2f);

        while (timer < maxTime)
        {
            timer += Time.deltaTime;

            float ratio = timer / maxTime;

            textBG.alpha = 1 - ratio;
            countdownText.alpha = 1 - ratio;
            countdownText.rectTransform.localScale = Vector3.Lerp(scale, targetScale, ratio);

            yield return null;
        }

        countdownText.rectTransform.localScale = scale;
    }

    private IEnumerator GamePause_AnimationTask(bool isPause)
    {
        float timer = 0;
        float maxTime = 0.3f;

        RectTransform rect = pauseMenu.transform as RectTransform;
        Vector3 oriScale = rect.localScale;

        if (isPause)
        {
            pauseMenu.gameObject.SetActive(true);

            pauseBG.blocksRaycasts = true;

            while (timer < maxTime)
            {
                timer += Time.unscaledDeltaTime;

                float ratio = timer / maxTime;

                pauseBG.alpha = ratio;
                rect.localScale = Vector3.Lerp(Vector3.zero, oriScale, ratio);

                yield return null;
            }

            rect.localScale = oriScale;
        }
        else
        {
            while (timer < maxTime)
            {
                timer += Time.unscaledDeltaTime;

                float ratio = 1 - (timer / maxTime);

                pauseBG.alpha = ratio;
                
                rect.localScale = Vector3.Lerp(Vector3.zero, oriScale, ratio);

                yield return null;
            }

            pauseBG.blocksRaycasts = false;
            pauseMenu.gameObject.SetActive(false);

            rect.localScale = oriScale;

        }
    }
}
