using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private Transform inventoryBox;
    [SerializeField] private Transform abilityBox;
    [SerializeField] private Transform equipmentBox;
    [Space]
    [SerializeField] private Transform playerStatBox;
    private Text healthText;
    private Text attackText;
    private Text defenseText;
    private Text moneyText;
    private Text xpText;
    private Text skillPointText;
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

    [Header("Sprites")]
    [SerializeField] private Sprite passiveBorder;
    [SerializeField] private Sprite activeBorder;

    [Header("Runtime Variables")]
    private Inventory inventory;
    private Player player;
    private readonly List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private readonly List<InventorySlot> equipmentSlots = new List<InventorySlot>();
    [Space]
    private readonly List<AbilitySlot> raceSlots = new List<AbilitySlot>();
    private readonly List<AbilitySlot> classSlots = new List<AbilitySlot>();
    private int selectedIndex = -1;
    private bool selectedEquipment = false;
    [Space]
    private PassiveSO selectedPassive;
    private AbilitySO selectedAbility;
    [Space]
    private int currentTab = 0;

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

        Transform classParent = abilityBox.GetChild(0);
        for (int i = 0; i < classParent.childCount; i++)
        {
            Image image = classParent.GetChild(i).GetChild(0).GetComponent<Image>();

            classSlots.Add(new AbilitySlot(image, selectionMarker, activeBorder, passiveBorder));
        }

        Transform raceParent = abilityBox.GetChild(1);
        for (int i = 0; i < raceParent.childCount; i++)
        {
            Image image = raceParent.GetChild(i).GetChild(0).GetComponent<Image>();

            raceSlots.Add(new AbilitySlot(image, selectionMarker, activeBorder, passiveBorder));
        }

        skillPointText = inventoryBox.transform.parent.GetChild(3).GetComponent<Text>();
    }

    public void ShowUI()
    {
        UpdateInventoryUI();
        UpdateProfileUI();
        player.UpdateInventoryStats();

        OpenTab(0);

        inventoryCanvas.SetActive(true);
    }

    public void OpenTab(int index)
    {
        currentTab = index;

        if (index == 0)
        {
            SelectItem(0);
            inventoryBox.gameObject.SetActive(true);
            abilityBox.gameObject.SetActive(false);

            skillPointText.gameObject.SetActive(false);
        }
        else
        {
            ShowItemInfo(null);

            selectionMarker.SetActive(false);
            abilityBox.gameObject.SetActive(true);
            inventoryBox.gameObject.SetActive(false);
            LoadAbilityUI();

            skillPointText.gameObject.SetActive(true);
        }
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

    public void SelectRaceAbility(int index)
    {
        raceSlots[index].SelectSlot();

        if (player.baseStat.startingAbilities.Count > index)
        {
            ShowAbilityInfo(player.baseStat.startingAbilities[index]);
            return;
        }

        index -= player.baseStat.startingAbilities.Count;

        if (player.baseStat.unlockablePassives.Count > index)
            ShowAbilityInfo(player.baseStat.unlockablePassives[index]);
        else
        {
            ShowItemInfo(null);
            selectionMarker.SetActive(false);
        }
    }

    public void SelectClassAbility(int index)
    {
        classSlots[index].SelectSlot();

        if (player.classStat.startingPassives.Count > index)
        {
            ShowAbilityInfo(player.classStat.startingPassives[index]);
            return;
        }

        index -= player.classStat.startingPassives.Count;

        if (player.classStat.unlockableAbilities.Count > index)
        {
            ShowAbilityInfo(player.classStat.unlockableAbilities[index]);
            return;
        }

        index -= player.classStat.unlockableAbilities.Count;

        if (player.classStat.unlockablePassives.Count > index)
            ShowAbilityInfo(player.classStat.unlockablePassives[index]);
        else
        {
            ShowItemInfo(null);
            selectionMarker.SetActive(false);
        }
    }

    public void UseItem()
    {
        if (currentTab == 0)
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
        else
        {
            if (selectedAbility)
                AbilityTree.instance.UnlockAbility(selectedAbility);
            else if (selectedPassive)
                AbilityTree.instance.UnlockPassive(selectedPassive);

            AbilityTree.instance.skillPoints--;
            skillPointText.text = "Points Available: " + AbilityTree.instance.skillPoints;

            LoadAbilityUI();
            ShowItemInfo(null);
        }
    }

    private void ShowItemInfo(Item item, bool disableButton = false)
    {

        if (item == null)
        {
            itemIcon.transform.parent.gameObject.SetActive(false);
            itemNameText.text = "";
            itemTypeText.text = "";
            itemDescriptionText.text = "";
            itemStatsText.text = "";

            useButtonText.text = ":)";
            useButton.interactable = false;
            // Clear all data
            return;
        }

        itemIcon.transform.parent.gameObject.SetActive(true);

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

    private void ShowAbilityInfo(AbilitySO data)
    {
        selectedPassive = null;
        selectedAbility = data;

        itemIcon.transform.parent.gameObject.SetActive(true);
        itemIcon.sprite = data.abilitySprite;
        itemNameText.text = data.abilityName;
        itemTypeText.text = "Active Ability";

        itemDescriptionText.text = data.abilityDescription;

        bool unlocked = AbilityTree.instance.IsAbilityUnlocked(data);

        if (unlocked)
            useButtonText.text = "UNLOCKED";
        else
        {
            if (AbilityTree.instance.skillPoints > 0)
                useButtonText.text = "UNLOCK";
            else
            {
                useButtonText.text = "NO POINTS";
                unlocked = true;
            }
        }


        useButton.interactable = !unlocked;
    }

    private void ShowAbilityInfo(PassiveSO data)
    {

        selectedAbility = null;
        selectedPassive = data;

        itemIcon.transform.parent.gameObject.SetActive(true);
        itemIcon.sprite = data.passiveSprite;
        itemNameText.text = data.passiveName;
        itemTypeText.text = "Passive Ability";

        itemDescriptionText.text = data.passiveDescription;

        bool unlocked = AbilityTree.instance.IsPassiveUnlocked(data);

        if (unlocked)
            useButtonText.text = "UNLOCKED";
        else
        {
            if (AbilityTree.instance.skillPoints > 0)
                useButtonText.text = "UNLOCK";
            else
            {
                useButtonText.text = "NO POINTS";
                unlocked = true;
            }
        }

        useButton.interactable = !unlocked;
    }

    private void LoadAbilityUI()
    {
        int raceIndex = 0;
        foreach (AbilitySO ability in player.baseStat.startingAbilities)
        {
            raceSlots[raceIndex].SetSlot(ability);
            raceIndex++;
        }

        foreach (PassiveSO passive in player.baseStat.unlockablePassives)
        {
            raceSlots[raceIndex].SetSlot(passive);
            raceIndex++;
        }


        int classIndex = 0;
        foreach (PassiveSO passive in player.classStat.startingPassives)
        {
            classSlots[classIndex].SetSlot(passive);
            classIndex++;
        }

        foreach (AbilitySO ability in player.classStat.unlockableAbilities)
        {
            classSlots[classIndex].SetSlot(ability);
            classIndex++;
        }

        foreach (PassiveSO passive in player.classStat.unlockablePassives)
        {
            classSlots[classIndex].SetSlot(passive);
            classIndex++;
        }

        skillPointText.text = "Points Available: " + AbilityTree.instance.skillPoints;
    }
}
