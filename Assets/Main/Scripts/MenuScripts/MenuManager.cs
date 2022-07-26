using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public TextAsset text;
    public GameObject optionsPage;
    public GameObject mainPage;

    public AudioMixerGroup Mixer;

    public void OnBeginClick()
    {
        SceneManager.LoadScene("Map");
    }

    public void OnOptionsClick()
    {
        optionsPage.SetActive(true);
        mainPage.SetActive(false);
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

    public void OnReturnClick()
    {
        mainPage.SetActive(true);
        optionsPage.SetActive(false);
    }

    public void OnChangedVolume(float volume)
    {

    }
}
