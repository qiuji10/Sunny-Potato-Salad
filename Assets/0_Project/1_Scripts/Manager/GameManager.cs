using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private Timer timeManager;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private CanvasGroup textBG;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMP_Text scoreText;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private CanvasGroup pauseBG;

    [Header("Game End Menu")]
    [SerializeField] private RectTransform endMenu;
    [SerializeField] private TMP_Text menuScoreText;
    [SerializeField] private TMP_Text menuHighScoreText;
    public static int score = 0;
    public static List<Transform> treasureChests = new List<Transform>();

    private void Awake()
    {
        score = 0;
        Timer.OnTimerStop += Timer_OnTimerStop;
    }

    private void OnDestroy()
    {
        score = 0;
        treasureChests.Clear();
        Timer.OnTimerStop -= Timer_OnTimerStop;
    }

    public void PauseGame()
    {
        playerController.StopAllCoroutines();
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

    [Button]
    private void Timer_OnTimerStop()
    {
        pauseButton.gameObject.SetActive(false);

        int highscore = PlayerPrefs.GetInt("highscore");

        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }

        highscore = PlayerPrefs.GetInt("highscore");

        countdownText.text = "TIME OVER";
        menuScoreText.text = score.ToString();
        menuHighScoreText.text = highscore.ToString();

        playerController.cantMove = true;
        StartCoroutine(GameEnd_AnimationTask());
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
        countdownText.text = "Go Go Goo!";
        pauseButton.gameObject.SetActive(true);
        playerController.cantMove = false;
        countdownText.fontSize = oriFontSize;
        timeManager.StartTimer();

        yield return new WaitForSeconds(1f);
        countdownText.text = "";
        
    }

    private void Update()
    {
        scoreText.text = $"Score\n{score}";
    }

    public static Transform FindNearestTreasureChest(Vector3 position, float minDistance)
    {
        if (treasureChests.Count == 0)
        {
            Debug.LogWarning("No treasure chests found!");
            return null;
        }

        Transform nearestTreasureChest = null;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < treasureChests.Count; i++)
        {
            float distance = Vector3.Distance(position, treasureChests[i].position);
            if (distance < nearestDistance && distance <= minDistance)
            {
                nearestTreasureChest = treasureChests[i];
                nearestDistance = distance;
            }
        }

        return nearestTreasureChest;
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

    private IEnumerator GameEnd_AnimationTask()
    {
        float timer = 0;
        float maxTime = 0.3f;

        RectTransform gameStateTransform = textBG.transform as RectTransform;
        Vector2 oriPosition = gameStateTransform.anchoredPosition;
        Vector2 destination = new Vector2(0, 240);

        while (timer < maxTime)
        {
            timer += Time.deltaTime;

            float ratio = timer / maxTime;

            textBG.alpha = ratio;
            countdownText.alpha = ratio;
            
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        timer = 0;
        maxTime = 0.3f;

        endMenu.gameObject.SetActive(true);

        while (timer < maxTime)
        {
            timer += Time.deltaTime;

            float ratio = timer / maxTime;

            endMenu.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, ratio);

            gameStateTransform.anchoredPosition = Vector3.Lerp(oriPosition, destination, ratio);

            yield return null;
        }

        textBG.alpha = 1;
        countdownText.alpha = 1;
    }
}
