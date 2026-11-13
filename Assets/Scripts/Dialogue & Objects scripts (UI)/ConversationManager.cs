using UnityEngine;
using TMPro;
using System.Collections;

public class ConversationManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;

        [TextArea(3, 5)]
        public string dialogue;
    }

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI continuePrompt;

    [Header("Character Portraits")]
    public UnityEngine.UI.Image speakerPortrait;

    public Sprite luckPortrait;
    public Sprite danePortrait;
    public Sprite spikePortrait;

    [Header("Speaker Colours")]
    public Color luckColour = new Color(1f, 0.75f, 0.2f);
    public Color daneColour = new Color(0.3f, 0.65f, 1f);
    public Color spikeColour = new Color(0.9f, 0.25f, 0.25f);

    [Header("Ending UI")]
    public GameObject toBeContinuedPanel;

    [Header("Camera Controller")]
    public CutsceneCameraController cameraController;

    [Header("Conversation Settings")]
    public float startDelay = 1.5f;
    public float typingSpeed = 0.035f;

    [Header("Conversation Lines")]
    public DialogueLine[] conversation;

    private int currentLine = 0;
    private bool conversationStarted = false;
    private bool conversationEnded = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    private void Start()
    {
        dialoguePanel.SetActive(true);
        toBeContinuedPanel.SetActive(false);

        dialogueText.text = "";
        characterName.text = "";

        if (continuePrompt != null)
            continuePrompt.gameObject.SetActive(false);

        StartCoroutine(BeginConversation());
    }

    private IEnumerator BeginConversation()
    {
        yield return new WaitForSeconds(startDelay);

        conversationStarted = true;

        cameraController.ShowWideShot();

        DisplayLine();
    }

    private void Update()
    {
        if (!conversationStarted || conversationEnded)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                CompleteCurrentLine();
            }
            else
            {
                NextLine();
            }
        }
    }

    private void DisplayLine()
    {
        if (currentLine >= conversation.Length)
        {
            EndConversation();
            return;
        }

        string speaker = conversation[currentLine].speakerName.Trim().ToUpper();

        characterName.text = conversation[currentLine].speakerName;

        switch (speaker)
        {
            case "LUCK":
            case "LADY LUCK":
                characterName.color = luckColour;
                speakerPortrait.sprite = luckPortrait;
                cameraController.ShowLuck();
                break;

            case "GENERAL DANE":
                characterName.color = daneColour;
                speakerPortrait.sprite = danePortrait;
                cameraController.ShowDane();
                break;

            case "COLONEL SPIKE":
                characterName.color = spikeColour;
                speakerPortrait.sprite = spikePortrait;
                cameraController.ShowSpike();
                break;
        }

        if (continuePrompt != null)
            continuePrompt.gameObject.SetActive(false);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue());
    }

    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";

        string fullText = conversation[currentLine].dialogue;

        foreach (char letter in fullText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;

        if (continuePrompt != null)
            continuePrompt.gameObject.SetActive(true);
    }

    private void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = conversation[currentLine].dialogue;

        isTyping = false;
        typingCoroutine = null;

        if (continuePrompt != null)
            continuePrompt.gameObject.SetActive(true);
    }

    private void NextLine()
    {
        currentLine++;
        DisplayLine();
    }

    private void EndConversation()
    {
        conversationEnded = true;

        dialoguePanel.SetActive(false);
        toBeContinuedPanel.SetActive(true);
    }
}