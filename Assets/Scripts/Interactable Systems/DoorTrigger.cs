using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTrigger : MonoBehaviour
{
    [Header("UI Settings")]
    public string openPrompt = "Press [E] to Open Door";
    public string closePrompt = "Press [E] to Close Door";
    public TextMeshProUGUI uiText;

    [Header("Door Animation Settings")]
    public Animator doorAnimator;
    public string boolParameterName = "IsOpen";

    private bool isPlayerInRange = false;
    private bool isOpen = false;

    private void Update()
    {
        // Listen for key press while inside trigger area
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleDoor();
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;

        // Trigger animator parameter
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(boolParameterName, isOpen);
        }

        // Update UI prompt
        if (uiText != null)
        {
            uiText.text = isOpen ? closePrompt : openPrompt;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (uiText != null)
            {
                uiText.text = isOpen ? closePrompt : openPrompt;
                uiText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (uiText != null)
            {
                uiText.gameObject.SetActive(false);
            }
        }
    }
}
