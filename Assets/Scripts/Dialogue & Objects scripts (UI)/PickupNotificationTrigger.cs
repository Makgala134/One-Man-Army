using UnityEngine;

public class PickupNotificationTrigger : MonoBehaviour
{
    [Header("Notification")]
    public PickupNotificationManager notificationManager;

    [TextArea(2, 4)]
    public string pickupMessage;

    [Header("Pickup Detection")]
    public Transform holdPoint;

    private bool notificationShown = false;

    private void LateUpdate()
    {
        if (notificationShown || holdPoint == null)
            return;

        if (transform.parent == holdPoint)
        {
            notificationShown = true;

            if (notificationManager != null)
            {
                notificationManager.ShowNotification(pickupMessage);
            }
        }
    }
}