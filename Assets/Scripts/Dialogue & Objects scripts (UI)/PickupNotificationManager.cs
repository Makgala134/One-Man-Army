using System.Collections;
using UnityEngine;
using TMPro;

public class PickupNotificationManager : MonoBehaviour
{
    [Header("Notification UI")]
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;

    [Header("Settings")]
    public float displayDuration = 3f;

    private Coroutine notificationCoroutine;

    private void Start()
    {
        notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if (notificationCoroutine != null)
        {
            StopCoroutine(notificationCoroutine);
        }

        notificationCoroutine = StartCoroutine(DisplayNotification(message));
    }

    private IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(displayDuration);

        notificationPanel.SetActive(false);
        notificationCoroutine = null;
    }
}