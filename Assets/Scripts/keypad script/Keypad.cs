using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Keypad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Answer;
    [SerializeField] private Animator DoorTrigger;
    [SerializeField] private GameObject keypadPanel;
    [SerializeField] private MonoBehaviour FPController; // Reference to the player's look script
    [SerializeField] private MonoBehaviour playerLook; // Reference to the player's look script
    [SerializeField] private float doorOpenTime = 1f; // Time in seconds for the door to stay open before closing
    private string AnswerText = "1234";
    private bool keypadOpen = false;

    private void Start()
    {
        keypadPanel.SetActive(false);
        Answer.text = "";
    }
    //collider trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenKeypad();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseKeypad();
        }

    }

    public void Number(int number)
    {
        Answer.text += number.ToString();
        Debug.Log("Number pressed: " + number);

        //clears the invalid message when a number is pressed
        if (Answer.text == "Invalid") Answer.text = ""; 

        if (Answer.text.Length < AnswerText.Length)
            Answer.text += number.ToString();
        
     }

    public void Clear ()
    {
        Answer.text = "";
    }
    public void Enter()
    {
        if (Answer.text == AnswerText)
        {
            Debug.Log("Correct code entered!");
            DoorTrigger.SetBool("IsOpen", true);
            StartCoroutine("StopDoorAnimation");
            CloseKeypad();
          
            // Add your logic for the correct code here (e.g., open a door, trigger an event, etc.)
        }
        else
        {
            Debug.Log("Incorrect code. Try again.");
            Answer.text = "Invalid"; // Clear the input for another attempt
        }
    }

    private IEnumerator StopDoorAnimation()
    {
        yield return new WaitForSeconds(doorOpenTime);
        
    }

    void Update()

    {
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            Number(1);

        // keeps mouse free while the keypad is open 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
     

    public void OpenKeypad()
    {
        keypadOpen = true;
        keypadPanel.SetActive(true);
        Answer.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if(playerLook != null)
        {
            playerLook.enabled = false; // Disable the player's look script
        }
        
        if(FPController != null)
        {
            FPController.enabled = false; // Disable the player's look script
        }
    }

    public void CloseKeypad()
    {
        keypadOpen = false;
        keypadPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // playerLook.enabled = true;

        if(playerLook != null)
        {
            playerLook.enabled = true; // Enable the player's look script
        }
        if(FPController != null)
        {
            FPController.enabled = true; // Enable the player's look script
        }
    }
}
