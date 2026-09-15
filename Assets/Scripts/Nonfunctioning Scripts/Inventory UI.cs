using UnityEngine;

using UnityEngine.UI;

// Simple grid-based UI. Attach to a UI Panel (Canvas > Panel).
// Assign a "slotPrefab" that has an Image (icon) and a Text/TMP_Text (quantity) on it.
public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventory;
    public Transform slotContainer;   // parent with a GridLayoutGroup
    public GameObject slotPrefab;     // prefab: Image + Text child named "QuantityText"

    void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged += Refresh;
    }

    void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    void Start()
    {
        Refresh();
    }

    void Refresh()
    {
        // clear existing slot visuals
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var slot in inventory.slots)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);

            Image icon = slotGO.GetComponentInChildren<Image>();
            if (icon != null && slot.item.icon != null)
                icon.sprite = slot.item.icon;

            Text qtyText = slotGO.GetComponentInChildren<Text>();
            if (qtyText != null)
                qtyText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
        }
    }
}

