using System.Collections;
using UnityEngine;
using TMPro;

public class ObjectDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public int roomline;
    public float textSpeed = 0.05f;

    [Header("Thought Bubble")]
    public GameObject thoughtBubbleBox;

    private int index;
    private bool isTyping;
    private bool isDialogueActive;

    public bool IsDialogueActive => isDialogueActive;

    private void Awake()
    {
        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        if (thoughtBubbleBox != null)
        {
            thoughtBubbleBox.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialogue()
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines assigned to " + gameObject.name);
            return;
        }

        if (textComponent == null)
        {
            Debug.LogWarning("Text Component has not been assigned to " + gameObject.name);
            return;
        }

        StopAllCoroutines();

        index = 0;
        isDialogueActive = true;

        textComponent.text = string.Empty;

        if (thoughtBubbleBox != null)
        {
            thoughtBubbleBox.SetActive(true);
        }

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();

        isTyping = false;
        isDialogueActive = false;

        if (textComponent != null)
        {
            textComponent.text = string.Empty;
        }

        if (thoughtBubbleBox != null)
        {
            thoughtBubbleBox.SetActive(false);
        }
    }
}

