using UnityEngine;
using TMPro;
public class ObjectUpdatedDialogueTrigger : MonoBehaviour

{
    [Header("References")]
    public ObjectDialogue objectDialogue;
    public GameObject player;
    public TextMeshProUGUI interactionPrompt;

    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.F;
    public string promptMessage = "Press F to Investigate";

    private bool playerInRange;
    private bool objectPickedUp;

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (objectPickedUp || !playerInRange || objectDialogue == null)
        {
            HidePrompt();
            return;
        }

        if (objectDialogue.IsDialogueActive)
        {
            HidePrompt();
            return;
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.text = promptMessage;
            interactionPrompt.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(interactionKey))
        {
            HidePrompt();
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
            HidePrompt();
        }
    }

    private void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }

    // Hide the prompt when the object is disabled or destroyed
    private void OnDisable()
    {
        HidePrompt();
    }

    private void OnDestroy()
    {
        HidePrompt();
    }
}

