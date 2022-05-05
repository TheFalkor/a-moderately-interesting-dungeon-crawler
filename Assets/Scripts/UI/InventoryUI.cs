using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private Transform inventoryBox;
    [SerializeField] private GameObject inventoryCanvas;
    [Space]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemStatsText;

    [Header("Runtime Variables")]
    private Inventory inventory;
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    [Header("Singleton")]
    public static InventoryUI instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();

        for (int i = 0; i < inventoryBox.childCount; i++)
        {
            Image image = inventoryBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = inventoryBox.GetChild(i).GetChild(1).GetComponent<Text>();

            inventorySlots.Add(new InventorySlot(image, text));
        }
    }

    public void ShowUI()
    {
        UpdateUI();
        ShowItemInfo(null);
        inventoryCanvas.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        DungeonManager.instance.RemoveRestrictions();
    }

    public void UpdateUI()
    {
        foreach (InventorySlot slot in inventorySlots)
            slot.ClearSlot();

        for (int i = 0; i < inventory.items.Length; i++)
        {
            Item item = inventory.items[i];

            if (item == null)
                continue;

            inventorySlots[i].SetSlot(item);
        }
    }

    public void SelectItem(int index)
    {
        if (inventory.items[index] == null)
            ShowItemInfo(null);

        ShowItemInfo(inventory.items[index]);
        // Highlight the selected one with border thingie
    }

    public void ShowItemInfo(Item item)
    {
        if (item == null)
        {
            itemIcon.gameObject.SetActive(false);
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemStatsText.text = "";
            // Clear all data
            return;
        }

        itemIcon.gameObject.SetActive(true);

        itemIcon.sprite = item.itemSprite;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;

        switch (item.itemType)
        {
            case ItemType.WEAPON:
                //
                break;

            case ItemType.ARMOR:
                //
                break;

            case ItemType.ACCESSORY:
                //
                break;

            case ItemType.CONSUMABLE:
                itemStatsText.text = "HEAL: " + ((Consumable)item).consumableValue;
                break;

            case ItemType.THROWABLE:
                //
                break;
        }

    }

}
