using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("UI Reference")]
    public TextMeshProUGUI textMeshPro;

    private Camera mainCamera;
    private IInteractable currentInteractable;

    private void Start()
    {
        mainCamera = Camera.main;

        if (textMeshPro != null)
            textMeshPro.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckForInteractable();

        // Check for key press using New Input System
        if (currentInteractable != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();
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

    private void ClearInteraction()
    {
        currentInteractable = null;
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

}
