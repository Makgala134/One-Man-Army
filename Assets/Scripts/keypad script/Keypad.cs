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
    private string AnswerText = "1234";
  

    public void Number(int number)
    {
        Answer.text += number.ToString();
        Debug.Log("Number pressed: " + number);
    }
    public void Enter()
    {
        if (Answer.text == AnswerText)
        {
            Debug.Log("Correct code entered!");
            DoorTrigger.SetBool("IsOpen", true);
            StartCoroutine("StopDoorAnimation");

          
            // Add your logic for the correct code here (e.g., open a door, trigger an event, etc.)
        }
        else
        {
            Debug.Log("Incorrect code. Try again.");
            Answer.text = "Invalid"; // Clear the input for another attempt
        }
    }

void Update()
       
    {
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            Number(1);
    }

    public void OpenKeypad()
    {
        keypadPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // also disable your look script so the camera doesn't spin:
        // playerLook.enabled = false;
    }

    public void CloseKeypad()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // playerLook.enabled = true;
    }
}
