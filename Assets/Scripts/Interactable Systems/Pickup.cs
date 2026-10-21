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
    public AudioClip pickupSound;

    [Range(0f, 1f)] public float volume = 1f;
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
        // If the player is already holding this item, we don't want to show the pickup prompt again
        else if (other.CompareTag("Player") && isBeingHeld)
        {
            isPlayerInRange = false;
            if (uiText != null)
            {
                uiText.gameObject.SetActive(false);
            }
        }
        
        if (other.CompareTag("Player"))
        {
            // Play the sound in 3D space at the item's location.
            // This creates a temporary audio player so the sound keeps playing 
            // even after this specific game object is destroyed.
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
        }
            
            // Put any inventory or score tracking logic here
            Debug.Log("Item Picked Up!");

            // Remove the pickup object from the scene
           
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

        //sound for pick up object
        //AudioManager.instance.Play("PickUp");


        //Drag your audio file into this slot in the Inspector
   

    // Adjust the volume of the pickup sound (0.0 to 1.0)
 

    
    

    private void Drop()
    {
        isBeingHeld = false;

        // Unparent object
        transform.SetParent(null);

        // Restore physics
        if (rb != null) rb.isKinematic = false;


    }
}
