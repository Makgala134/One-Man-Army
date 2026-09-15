using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    [TextArea] public string description;
    public Sprite icon;
    public GameObject worldPrefab;   // This will be used to show the item back in the world when dropped
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
//the code that runs the inventory system for the player.
//It allows the player to add and remove items from their inventory, as well as check if they have a certain item or if the inventory is full.
{
    [Header("Settings")]
    public int inventorySize = 4;

    [Header("Runtime (view only)")]
    public List<InventorySlot> slots = new List<InventorySlot>();

       public event Action OnInventoryChanged;

   
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;
        //allows for players keep the items stacking instead of taking up too many slots in the inventory.

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

   
        while (quantity > 0)
        {
            if (slots.Count >= inventorySize)
            {
                OnInventoryChanged?.Invoke();
                return false; //this code stops new items from being added once the invenotry is full
            }

            int amountToAdd = item.isStackable ? Mathf.Min(quantity, item.maxStackSize) : 1;
            slots.Add(new InventorySlot(item, amountToAdd));
            quantity -= amountToAdd;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    
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


