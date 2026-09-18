using TMPro.Examples;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite item;
    public GameObject itemPrefab; //ground item we can pick up
    public int maxStackSize;
    public GameObject handItemPrefab; //this will be for the object our that will be handheld
}
