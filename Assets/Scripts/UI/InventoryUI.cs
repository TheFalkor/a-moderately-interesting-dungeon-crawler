using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private Transform inventoryBox;
    [SerializeField] private Transform equipmentBox;
    [Space]
    [SerializeField] private GameObject selectionMarker;
    [Space]
    [SerializeField] private GameObject inventoryCanvas;
    [Space]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemStatsText;
    [SerializeField] private Button useButton;
    private Text useButtonText;

    [Header("Runtime Variables")]
    private Inventory inventory;
    private readonly List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private readonly List<InventorySlot> equipmentSlots = new List<InventorySlot>();
    private int selectedIndex = -1;
    private bool selectedEquipment = false;

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

            inventorySlots.Add(new InventorySlot(image, text, selectionMarker));
        }

        for (int i = 0; i < equipmentBox.childCount; i++)
        {
            Image image = equipmentBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = equipmentBox.GetChild(i).GetChild(1).GetComponent<Text>();

            equipmentSlots.Add(new InventorySlot(image, text, selectionMarker));
        }

        useButtonText = useButton.transform.GetChild(0).GetComponent<Text>();
    }

    public void ShowUI()
    {
        UpdateInventoryUI();
        ShowItemInfo(null);
        selectionMarker.SetActive(false);
        inventoryCanvas.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        DungeonManager.instance.RemoveRestrictions();
    }

    public void UpdateInventoryUI()
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

    public void UpdateEquipmentUI()
    {
        foreach (InventorySlot slot in equipmentSlots)
            slot.ClearSlot();

        if (inventory.equippedWeapon != null)
            equipmentSlots[0].SetSlot(inventory.equippedWeapon);

        if (inventory.equippedArmor != null)
            equipmentSlots[1].SetSlot(inventory.equippedArmor);

        if (inventory.equippedAccessory != null)
            equipmentSlots[2].SetSlot(inventory.equippedAccessory);
    }

    public void SelectItem(int index)
    {
        inventorySlots[index].SelectSlot();

        if (inventory.items[index] == null)
        {
            ShowItemInfo(null);
            selectedIndex = -1;
            return;
        }

        selectedIndex = index;
        ShowItemInfo(inventory.items[index]);
        // Highlight the selected one with border thingie
    }

    public void SelectEquipment(int index)
    {
        equipmentSlots[index].SelectSlot();
        Item item = null;

        if (index == 0)
        {
            item = inventory.equippedWeapon;
        }
        else if (index == 1)
        {
            item = inventory.equippedArmor;
        }
        else if (index == 2)
        {
            item = inventory.equippedAccessory;
        }

        ShowItemInfo(item, true);
    }

    public void UseItem()
    {
        if (selectedEquipment)
        {
            int slotIndex = inventory.EquipItem(selectedIndex);
            SelectEquipment(slotIndex);
            //ShowItemInfo(inventory.items[selectedIndex], true);
            //inventory.EquipItem(selectedIndex);
        }
        else
        {
            inventory.UseItem(selectedIndex);
        }
    }

    private void ShowItemInfo(Item item, bool disableButton = false)
    {

        if (item == null)
        {
            itemIcon.gameObject.SetActive(false);
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemStatsText.text = "";

            useButtonText.text = ":)";
            useButton.interactable = false;
            // Clear all data
            return;
        }

        itemIcon.gameObject.SetActive(true);

        itemIcon.sprite = item.itemSprite;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemStatsText.text = "";

        switch (item.itemType)
        {
            case ItemType.WEAPON:
                //
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.ARMOR:
                //
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.ACCESSORY:
                //
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.CONSUMABLE:
                itemStatsText.text = "HEAL: " + ((Consumable)item).consumableValue;
                useButtonText.text = "USE ITEM";
                selectedEquipment = false;
                break;

            case ItemType.THROWABLE:
                //
                useButtonText.text = "CANNOT USE";
                selectedEquipment = false;
                disableButton = true;
                break;
        }

        if (disableButton && item.itemType != ItemType.THROWABLE)
            useButtonText.text = "EQUIPPED";

        useButton.interactable = !disableButton;
    }

}
