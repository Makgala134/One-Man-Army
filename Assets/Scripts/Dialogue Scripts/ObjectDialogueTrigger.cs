using UnityEngine;
using TMPro;

public class ObjectDialogueTrigger : MonoBehaviour
{
    [Header("References")]
    public ObjectDialogue objectDialogue;
    public GameObject player;
    public TextMeshProUGUI interactionPrompt;

    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.F;
    public string promptMessage = "Press F to Investigate";

    private bool playerInRange;

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange || objectDialogue == null)
            return;

        if (objectDialogue.IsDialogueActive)
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }

            return;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.text = promptMessage;

            if (!interactionPrompt.gameObject.activeSelf)
            {
                interactionPrompt.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(interactionKey))
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }

            objectDialogue.StartDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player != null && other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null && other.gameObject == player)
        {
            playerInRange = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.gameObject.SetActive(false);
            }
        }
    }
}

