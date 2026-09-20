using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CodeDoor : MonoBehaviour
{
    [Header("UI Settings")]
    public string openPrompt = "Press [E] to Open Door";
    public string closePrompt = "Press [E] to Close Door";
    public TextMeshProUGUI uiText;

    [Header("Door Object & Rotation Settings")]
    public Transform doorPivot; // Drag the door model/hinge here
    public float openAngle = 90f;
    public float speed = 5f;

    private bool isPlayerInRange = false;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        if (doorPivot != null)
        {
            closedRotation = doorPivot.localRotation;
            openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
        }
    }

    private void Update()
    {
        // Smoothly rotate door toward target
        if (doorPivot != null)
        {
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            doorPivot.localRotation = Quaternion.Slerp(doorPivot.localRotation, targetRotation, Time.deltaTime * speed);
        }

        // Toggle door on keypress
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;
            if (uiText != null)
            {
                uiText.text = isOpen ? closePrompt : openPrompt;
            }
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
