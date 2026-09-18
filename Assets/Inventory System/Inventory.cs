using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class Inventory : MonoBehaviour
{
    public ItemSO fileItem;
    public ItemSO remoteItem;

    public GameObject hotbarObject;
    public GameObject inventorySlotParent;

    public Image dragIcon;

    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private Slot draggedSlot = null;
    private bool isDragging = false;


    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>()); //this will get all the slots in the inventory and add them to the list
        hotbarSlots.AddRange(hotbarObject.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            AddItem(fileItem, 2);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            AddItem(remoteItem, 1);
        }
    }

    public void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;

        foreach (Slot slot in allSlots) //first we check if the item already exists in the inventory and if it does, we add to that stack
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= -amountToAdd;

                    if (remaining <= 0)
                        return;
                }
            }
        }


        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem()) //small reminder for myself ! = not 
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining); 
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if (remaining <= 0)
                    return;
            }
        }
        
        if(remaining > 0) //if there are still items left to add after trying all slots

        {
            Debug.Log("Your Inventory is full, could not add " + remaining + " " + itemToAdd.itemName);
        }

    }

    private void StartDrag()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Slot hovered = GetHoveredSlot();

            if(hovered != null && hovered.HasItem())
            {
                draggedSlot = hovered;
                isDragging = true;

                //show drag item
                //dragIcon.sprite = hovered.GetItem().icon;
                //dragIcon.colour = new Colour(1, 1, 1,1);
                dragIcon.enabled = true;
                dragIcon.sprite = draggedSlot.GetItem().item;
            }
        }
    }
    private Slot GetHoveredSlot()
    {
        foreach(Slot s in allSlots)
        {
            if (s.hovering)
                return s;
        }
        return null;
    }
}
