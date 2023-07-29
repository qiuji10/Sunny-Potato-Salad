using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public List<TransitionSettings> transitions;
    public float startDelay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string _sceneName)
    {
        TransitionManager.Instance().Transition(_sceneName, GetRandomSettings(), startDelay);
    }

    private TransitionSettings GetRandomSettings()
    {
        if (transitions == null || transitions.Count == 0)
        {
            Debug.LogError("No transition settings available.");
            return null;
        }

        int randomIndex = Random.Range(0, transitions.Count);
        return transitions[randomIndex];
    }
}
