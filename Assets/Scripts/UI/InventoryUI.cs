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
    [SerializeField] private Transform playerStatBox;
    private Text healthText;
    private Text attackText;
    private Text defenseText;
    private Text moneyText;
    private Text xpText;
    [Space]
    [SerializeField] private GameObject selectionMarker;
    [Space]
    [SerializeField] private GameObject inventoryCanvas;
    [Space]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemTypeText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemStatsText;
    [SerializeField] private Button useButton;
    private Text useButtonText;
    [Space]
    private Transform profileParent;
    private Image profileIcon;
    private Text raceText;
    private Text classText;

    [Header("Runtime Variables")]
    private Inventory inventory;
    private Player player;
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
        player = DungeonManager.instance.player;

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

        profileParent = equipmentBox.parent;
        profileIcon = profileParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        raceText = profileParent.GetChild(1).GetComponent<Text>();
        classText = profileParent.GetChild(2).GetComponent<Text>();

        healthText = playerStatBox.GetChild(0).GetComponent<Text>();
        attackText = playerStatBox.GetChild(1).GetComponent<Text>();
        defenseText = playerStatBox.GetChild(2).GetComponent<Text>();
        moneyText = playerStatBox.GetChild(3).GetComponent<Text>();
        xpText = playerStatBox.GetChild(4).GetComponent<Text>();
    }

    public void ShowUI()
    {
        UpdateInventoryUI();
        UpdateProfileUI();
        player.UpdateInventoryStats();
        SelectItem(0);
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

    public void UpdateProfileUI()
    {
        profileIcon.sprite = player.baseStat.entitySprite;

        raceText.text = player.baseStat.entityName;
        classText.text = "Lv. 0 " + player.classStat.className;
    }

    public void UpdatePlayerStats(int currentHealth, int maxHealth, int defense, int damage)
    {
        healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        attackText.text = "ATK: " + damage;
        defenseText.text = "DEF: " + defense;

        moneyText.text = "Money: 0";
        xpText.text = "XP: ?";
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
        }
        else
        {
            inventory.UseItem(selectedIndex);
            SelectItem(selectedIndex);
        }

        player.UpdateInventoryStats();
    }

    private void ShowItemInfo(Item item, bool disableButton = false)
    {

        if (item == null)
        {
            itemIcon.gameObject.SetActive(false);
            itemNameText.text = "";
            itemTypeText.text = "";
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
                itemTypeText.text = "Weapon";
                itemStatsText.text = "+" + ((Weapon)item).weaponDamage + " ATK";
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.ARMOR:
                //
                itemTypeText.text = "Armor";
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.ACCESSORY:
                //
                itemTypeText.text = "Accessory";
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.CONSUMABLE:
                itemTypeText.text = "Consumable";
                itemStatsText.text = "+" + ((Consumable)item).consumableValue + " HP";
                useButtonText.text = "USE ITEM";
                selectedEquipment = false;
                break;

            case ItemType.THROWABLE:
                //
                itemTypeText.text = "Throwable";
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
