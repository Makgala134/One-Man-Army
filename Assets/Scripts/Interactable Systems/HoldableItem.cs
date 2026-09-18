using UnityEngine;

public class HoldableItem : MonoBehaviour, IInteractable
{
    [Header("Item Details")]
    public string ItemName = "Object";

    private Rigidbody rb;
    private Collider itemCollider;
    private bool isBeingHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
    }

    public void Interact()
    {

    }

    public string GetInteractPrompt()
    {
        return isBeingHeld ? $"Press E to Drop {ItemName}" : $"Press E to Pick Up {ItemName}";
    }

    public void PickUp(Transform holdPoint)
    {
        isBeingHeld = true;

        // Disable physics while holding
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Disable collider so it doesn't block player vision or raycasts
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // Attach to player hold point
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isBeingHeld = false;

        // Unparent from player
        transform.SetParent(null);

        // Re-enable physics and colliders
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }
    }
}
