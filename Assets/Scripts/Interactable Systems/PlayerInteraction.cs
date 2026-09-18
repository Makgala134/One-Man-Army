using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;
    public Transform holdPoint;

    [Header("UI Reference")]
    public TextMeshProUGUI textMeshPro;

    private Camera mainCamera;
    private IInteractable currentInteractable;
    private HoldableItem currentlyHeldItem;

    private void Start()
    {
        mainCamera = Camera.main;

        if (textMeshPro != null)
            textMeshPro.gameObject.SetActive(false);
    }

    private void Update()
    {
        // If holding an item, handle dropping it
        if (currentlyHeldItem != null)
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = currentlyHeldItem.GetInteractPrompt();
                textMeshPro.gameObject.SetActive(true);
            }

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                DropHeldItem();
            }
            return;
        }

        // Otherwise, scan for interactable objects in front of the camera
        CheckForInteractable();

        if (currentInteractable != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // If the object is a holdable item, pick it up
            if (currentInteractable is HoldableItem holdable)
            {
                PickUpItem(holdable);
            }
            else
            {
                currentInteractable.Interact();
            }
        }
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            // Check if the object hit implements IInteractable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                if (textMeshPro != null)
                {
                    textMeshPro.text = currentInteractable.GetInteractPrompt();
                    textMeshPro.gameObject.SetActive(true);
                }
                return;
            }
        }

        // Clear interaction if not looking at anything interactable
        ClearInteraction();
    }

    private void PickUpItem(HoldableItem item)
    {
        currentlyHeldItem = item;
        currentlyHeldItem.PickUp(holdPoint);
        currentInteractable = null;
    }

    private void DropHeldItem()
    {
        if (currentlyHeldItem != null)
        {
            currentlyHeldItem.Drop();
            currentlyHeldItem = null;
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        currentInteractable = null;
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

}
