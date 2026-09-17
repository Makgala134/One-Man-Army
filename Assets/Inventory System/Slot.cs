using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovering;

    private ItemSO heldItem;
    private Image iconImage;
    private int itemAmount;
    private TextMeshProUGUI amountText;
    private GameObject itemIcon;

    private void Awake()
    {
        iconImage = transform.GetChild(0).GetComponent<Image>(); //this is the image that will show the item in the slot
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>(); //this is the text that will show the amount of items in the slot
  
        iconImage = transform.Find("IconImage")?.GetComponent<Image>();
        amountText = transform.Find("AmountText")?.GetComponent<TextMeshProUGUI>();

        if (iconImage == null)
            Debug.LogError($"[{name}] Slot could not find IconImage child.");
        if (amountText == null)
            Debug.LogError($"[{name}] Slot could not find AmountText child.");
    }

    public ItemSO GetItem()
    {
        return heldItem;
    }

    public int GetAmount()
    {
        return itemAmount;
    }

    public void SetItem(ItemSO item, int amount = 1)
    {
        heldItem = item;
        itemAmount = amount;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if (heldItem != null)
        {
            iconImage.enabled = true; //if there is an item in the slot, we will enable the image
            iconImage.sprite = heldItem.item; //we will set the image to the item sprite
            amountText.text = itemAmount.ToString(); //updating slot values
        }
        else
        {
            iconImage.enabled = false; //if there is no item in the slot, we will disable the image
            amountText.text = ""; //if there is no item in the slot, we will clear the text
        }
    }

    public int AddAmount(int amount) //this will add the amount of items to the slot
    {
        itemAmount += amount;
        UpdateSlot();
        return itemAmount;
    }

    public int RemoveAmount(int amount) //this will remove the amount of items from the slot
    {
        itemAmount -= amount;

        if(itemAmount <= 0)         
        {
            ClearSlot();
        }
        else
        {
            UpdateSlot();
        }
        return itemAmount;
    }

    public void ClearSlot() //this will clear the slot of any items
    {
        heldItem = null;
        itemAmount = 0;
        UpdateSlot();
    }

    public bool HasItem() //this will check if the slot has an item
    {
        return heldItem != null;
    }

    public void OnPointerEnter(PointerEventData eventData)//this will check if the mouse is hovering over the slot
    {
      hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
     hovering= false; 
    }
}
