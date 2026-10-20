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
    public float timeRemaining = 360f; // Total time in seconds
    public bool timerIsActive = true;


    [Header("Countdown Settings")]
    public float countdownStartTime = 10f; // Show the big countdown for the last 10 seconds


    [Header("Audio")]
    public AudioClip countdownSound; // Drag your countdown sound clip here
    public AudioSource audioSource; // Optional: one is added automatically if left empty
    private bool countdownSoundPlayed = false;

    [Header("UI Components")]
    public TextMeshProUGUI timerText; // Drag your UI text here
    public TextMeshProUGUI countdownText; // Optional: big centre-screen countdown text (leave empty to skip)
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        countdownSoundPlayed = false;

        // Make sure the timer is 6 minutes even if an old value was saved in the Inspector
        timeRemaining = 360f;

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);
    }
    void Update()
    {
        if (timerIsActive)
        {
            if (timeRemaining > 0)
            {
                // Subtract the time passed since the last frame
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                UpdateCountdown(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsActive = false;
                DisplayTime(timeRemaining);
                if(countdownText != null)
                    countdownText.gameObject.SetActive(false);
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

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        // String formatting "00" ensures numbers under 10 show with a leading zero (e.g., 05 instead of 5)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    // Shows 10, 9, 8 ... 1 during the last 10 seconds
    void UpdateCountdown(float timeLeft)
    {
        if (timeLeft <= countdownStartTime && timeLeft > 0)
        {
            int secondsLeft = Mathf.CeilToInt(timeLeft);

            // Turn the main timer red for the final stretch
            timerText.color = Color.red;

            // Play the countdown sound once when the 10 second mark is reached
            if (!countdownSoundPlayed && countdownSound != null)
            {
                audioSource.PlayOneShot(countdownSound);
                countdownSoundPlayed = true;
            }

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);
                countdownText.text = secondsLeft.ToString();
            }
        }
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
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }
    public class MainMenu : MonoBehaviour
    {
        public void PlayGmae()
        {
            SceneManager.LoadScene(0);
        }
    }
}

