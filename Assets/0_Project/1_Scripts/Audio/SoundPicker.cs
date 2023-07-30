using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundPicker : MonoBehaviour
{
    public static int index;

    public AudioData bgmData;

    private void Start()
    {
        index = PlayerPrefs.GetInt("MusicPreferences");

        Play();
    }

    [Button]
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

        Play();
    }

    private void Play()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (index == 0)
            {
                PlayBGM(bgmData, "InMenu1");
            }
            else if (index == 1)
            {
                PlayBGM(bgmData, "InMenu2");
            }
        }
        else
        {
            if (index == 0)
            {
                PlayBGM(bgmData, "InGame1");
            }
            else if (index == 1)
            {
                PlayBGM(bgmData, "InGame2");
            }
        }
    }

    public void PlayBGM(AudioData data, string name)
    {
        AudioManager.instance.PlayBGM(data, name);
    }
}
