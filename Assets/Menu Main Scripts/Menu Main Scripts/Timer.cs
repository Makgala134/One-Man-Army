using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 300f; // Total time in seconds
    public bool timerIsActive = true;

    [Header("UI Components")]
    public TextMeshProUGUI timerText; // Drag your UI text here

    void Update()
    {
        if (timerIsActive)
        {
            if (timeRemaining > 0)
            {
                // Subtract the time passed since the last frame
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsActive = false;
                DisplayTime(timeRemaining);
                // Add your custom game over or event logic here
                OnTimerEnd();
            }
        }
    }

    // Formats the raw float seconds into Minutes:Seconds layout
    void DisplayTime(float timeToDisplay)
    {
        // Avoid negative display values
        if (timeToDisplay < 0) timeToDisplay = 0;

        float minutes = Mathf.FloorToInt(timeToDisplay / 30);
        float seconds = Mathf.FloorToInt(timeToDisplay % 30);

        // String formatting "00" ensures numbers under 10 show with a leading zero (e.g., 05 instead of 5)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnTimerEnd()
    {
        SceneManager.LoadScene(0); // Execute whatever happens when time hits 0
    }
    public class MainMenu : MonoBehaviour
    {
        public void PlayGmae()
        {
            SceneManager.LoadScene(0);
        }
    }
}

    