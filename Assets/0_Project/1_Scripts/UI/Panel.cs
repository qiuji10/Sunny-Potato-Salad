using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private CanvasGroup group;
    private RectTransform rect;
    private Queue<PanelData> requestQueue = new Queue<PanelData>();

    private struct PanelData
    {
        public Sprite sprite;
        public string title;
        public string description;
    }

    private void Awake()
    {
        rect = transform as RectTransform;
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    public void EnqueuePanel(Sprite sprite, string title, string description)
    {
        PanelData panelData = new PanelData()
        {
            sprite = sprite,
            title = title,
            description = description
        };

        requestQueue.Enqueue(panelData);

        if (requestQueue.Count == 1)
        {
            StartCoroutine(ProcessPanelQueue());
        }
    }

    private IEnumerator ProcessPanelQueue()
    {
        while (requestQueue.Count > 0)
        {
            PanelData panelData = requestQueue.Peek();

            image.sprite = panelData.sprite;
            this.title.text = panelData.title;
            this.description.text = panelData.description;

            yield return StartCoroutine(OpenPanelAnimation());

            yield return new WaitForSeconds(3f);

            yield return StartCoroutine(ClosePanelAnimation());
            requestQueue.Dequeue();
        }
    }

    private IEnumerator OpenPanelAnimation()
    {
        Vector3 oriPosition = rect.anchoredPosition;
        float timer = 0;
        float maxTime = 0.3f;

        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            float ratio = timer / maxTime;
            group.alpha = ratio;
            rect.anchoredPosition = Vector3.Lerp(Vector3.zero, oriPosition, ratio);
            yield return null;
        }
    }

    private IEnumerator ClosePanelAnimation()
    {
        Vector3 oriPosition = rect.anchoredPosition;
        float timer = 0;
        float maxTime = 0.3f;

        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            float ratio = 1 - (timer / maxTime);
            group.alpha = ratio;
            rect.anchoredPosition = Vector3.Lerp(Vector3.zero, oriPosition, ratio);
            yield return null;
        }

        rect.anchoredPosition = oriPosition;
    }
}
