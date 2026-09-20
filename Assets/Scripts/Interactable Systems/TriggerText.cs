using TMPro;
using UnityEngine;

public class TriggerText : MonoBehaviour
{
    [Header("UI Settings")]
    [TextArea] public string textToShow = "Press [E] to Open Door";
    public TextMeshProUGUI uiText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && uiText != null)
        {
            uiText.text = textToShow;
            uiText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }
    }
}
