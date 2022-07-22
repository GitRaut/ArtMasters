using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextAsset text;

    public void OnBeginClick()
    {
        SceneManager.LoadScene("Map");
    }

    public void OnOptionsClick()
    {
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
