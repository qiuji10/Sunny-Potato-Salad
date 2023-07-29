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

    private void Awake()
    {
        rect = transform as RectTransform;
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }

    public void SetPanel(Sprite sprite, string title, string description)
    {
        image.sprite = sprite;
        this.title.text = title;
        this.description.text = description;
    }

    [Button]
    public void OpenPanel()
    {
        StartCoroutine(OpenPanel_AnimationTask());
    }

    private IEnumerator OpenPanel_AnimationTask()
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

        yield return new WaitForSeconds(3f);

        timer = 0;

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
