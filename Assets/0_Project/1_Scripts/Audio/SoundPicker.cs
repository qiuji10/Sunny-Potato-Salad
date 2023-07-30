using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPicker : MonoBehaviour
{
    public static int index;

    private void Awake()
    {
        index = PlayerPrefs.GetInt("MusicPreferences");
    }

    public void SwitchMusic()
    {
        if (index == 0)
        {
            index = 1;
        }
        else if (index == 1)
        {
            index = 0;
        }

        PlayerPrefs.SetInt("MusicPreferences", index);
    }

    public static void PlayBGM(AudioData data, string name)
    {
        AudioManager.instance.PlayBGM(data, name);
    }
}
