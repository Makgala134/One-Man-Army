using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour, Interactable
{
    [Header("Item Data")]
    public ItemData item;
    public int quantity = 1;

    [Header("Optional")]
    public bool destroyOnPickup = true;

    public string InteractionPrompt => item != null ? $"Pick up {item.itemName}" : "Pick up";

    public void Interact(GameObject interactor)
    {
        if (item == null)
        {
            Debug.LogWarning($"{name} has no ItemData assigned.");
            return;
        }

        InventorySystem inventory = interactor.GetComponent<InventorySystem>();
        if (inventory == null)
        {
            Debug.LogWarning("Interactor has no InventorySystem component.");
            return;
        }

        bool added = inventory.AddItem(item, quantity);

        if (added)
        {
            if (destroyOnPickup)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventory full — could not pick up " + item.itemName);
        }
    }
}
