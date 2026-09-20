using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [Header("UI & Pickup Settings")]
    public string promptMessage = "Press [E] to Pick Up";
    public TextMeshProUGUI uiText;
    public Transform holdPoint;

    private Rigidbody rb;
    private Collider itemCollider;
    private bool isPlayerInRange = false;
    private bool isBeingHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        // Only listen for key presses if the player is in range OR if the player is holding this item
        if ((isPlayerInRange || isBeingHeld) && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isBeingHeld)
            {
                PickUp();
            }
            else
            {
                Drop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect player entering trigger
        if (other.CompareTag("Player") && !isBeingHeld)
        {
            isPlayerInRange = true;
            if (uiText != null)
            {
                uiText.text = promptMessage;
                uiText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detect player leaving trigger
        if (other.CompareTag("Player") && !isBeingHeld)
        {
            isPlayerInRange = false;
            if (uiText != null)
            {
                uiText.gameObject.SetActive(false);
            }
        }
    }

    private void PickUp()
    {
        isBeingHeld = true;
        isPlayerInRange = false;

        // Hide UI text
        if (uiText != null) uiText.gameObject.SetActive(false);

        // Turn off physics
        if (rb != null) rb.isKinematic = true;

        // Parent object to the HoldPoint
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Drop()
    {
        isBeingHeld = false;

        // Unparent object
        transform.SetParent(null);

        // Restore physics
        if (rb != null) rb.isKinematic = false;
    }
}
