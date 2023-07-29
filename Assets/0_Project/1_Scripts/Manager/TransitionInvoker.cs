using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInvoker : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        StageManager.Instance.LoadScene(sceneName);
    }
}
