using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("How To Play")]
    public GameObject howToPlayPanel;

    public void PlayGmae()
    {
        SceneManager.LoadScene(2);
    }


    public void QuitGame()

    {
        Application.Quit();
        Debug.Log("The Game is Quitting");
    }
    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

}


