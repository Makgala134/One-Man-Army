using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.ComponentModel.Design.Serialization;



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


    private int lastSecond = -1; // To track the last second displayed for countdown

    private bool countdownSoundPlayed = false;




    [Header("UI Components")]
    public TextMeshProUGUI timerText; // Drag your UI text here
    public TextMeshProUGUI countdownText; // Optional: big centre-screen countdown text (leave empty to skip)

    [Header("Game Over")]
    public GameOverManager gameOverManager;


    //[Header("ScreenFlash Settings")]
    //public Image flashimage;
    // public Color flashcolour = new Color(1f, 0f, 0f, 1f); // Red with 100% opacity
    //public float flashDuration = 0.5f; // Duration of the flash in seconds
    //public GameObject ScreenFlash; // Drag your UI image here (leave empty to skip)
    // private bool isFlashing = false;
    //private Coroutine flashCoroutine;

    [SerializeField] ScreenFlash screenFlash; // Reference to the ScreenFlash script
    [SerializeField] Color newColor = Color.red; // Color for the flash effect
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
        timeRemaining = 60f;

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

               // if (timeRemaining <= countdownStartTime && !isFlashing)
                {
                    // Start the screen flash effect
                    //flashCoroutine = StartCoroutine(ScreenFlash());
                }
            }
            else
            {
                Debug.Log("Time has run out!");

                timeRemaining = 0;

                timerIsActive = false;

                DisplayTime(timeRemaining); 

                if (countdownText != null)

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

            //only runs once per second
            if (secondsLeft != lastSecond)
            {
                lastSecond = secondsLeft;
                // Trigger the screen flash effect
                if (screenFlash != null)
                {
                    screenFlash.StartFlash(0.25f,0.51f, newColor); // Adjust duration and max alpha as needed
                }
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
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("Game Over Manager has not been assigned!");
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

    