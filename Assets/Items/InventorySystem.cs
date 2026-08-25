using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

// Attach this to your Player GameObject.
[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    [TextArea] public string description;
    public Sprite icon;
    public GameObject worldPrefab;   // prefab used if you drop the item back into the world
    public bool isStackable = true;
    public int maxStackSize = 99;
}

public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class InventorySystem : MonoBehaviour
{
    [Header("Settings")]
    public int inventorySize = 20;

    [Header("Runtime (view only)")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    // Subscribe to this from UI to refresh display whenever inventory changes
    public event Action OnInventoryChanged;

    /// <summary>
    /// Attempts to add an item. Returns true if it was fully added.
    /// Stacks onto existing slots first, then fills empty slots.
    /// </summary>
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        // Try to stack onto existing slots
        if (item.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int spaceLeft = item.maxStackSize - slot.quantity;
                    int amountToAdd = Mathf.Min(spaceLeft, quantity);
                    slot.quantity += amountToAdd;
                    quantity -= amountToAdd;

                    if (quantity <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
        }

        // Fill new slots for whatever quantity remains
        while (quantity > 0)
        {
            if (slots.Count >= inventorySize)
            {
                OnInventoryChanged?.Invoke();
                return false; // inventory full, partial add may have happened
            }

            int amountToAdd = item.isStackable ? Mathf.Min(quantity, item.maxStackSize) : 1;
            slots.Add(new InventorySlot(item, amountToAdd));
            quantity -= amountToAdd;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Removes up to 'quantity' of the given item. Returns true if the full amount was removed.
    /// </summary>
    public bool RemoveItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        int remaining = quantity;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i].item != item) continue;

            int amountToRemove = Mathf.Min(slots[i].quantity, remaining);
            slots[i].quantity -= amountToRemove;
            remaining -= amountToRemove;

            if (slots[i].quantity <= 0)
                slots.RemoveAt(i);

            if (remaining <= 0) break;
        }

        OnInventoryChanged?.Invoke();
        return remaining <= 0;
    }

    public int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach (var slot in slots)
            if (slot.item == item) count += slot.quantity;
        return count;
    }

    public bool HasItem(ItemData item, int quantity = 1)
    {
        return GetItemCount(item) >= quantity;
    }

    public bool IsFull => slots.Count >= inventorySize;
}

public interface Interactable
{
    string InteractionPrompt { get; }
    void Interact(GameObject interactor);
}


