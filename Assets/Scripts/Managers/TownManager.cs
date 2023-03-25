using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownManager : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private Transform equipablesBox;
    [SerializeField] private Transform consumablesBox;
    [SerializeField] private Transform sellBox;
    [SerializeField] private Transform equipmentBox;
    [Space]
    [SerializeField] private Transform playerStatBox;
    [Space]
    [SerializeField] private Button equipablesTabButton;
    [SerializeField] private Button consumablesTabButton;
    [SerializeField] private Button sellTabButton;

    private Text healthText;
    private Text attackText;
    private Text defenseText;
    private Text moneyText;

    [Space]
    [SerializeField] private GameObject selectionMarker;
    [Space]
    [SerializeField] private GameObject townCanvas;
    [Space]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemTypeText;
    [SerializeField] private Text itemDescriptionText;
    [Space]
    [SerializeField] private GameObject itemStatParent;
    [Space]
    [SerializeField] private Button interactButton;
    private Text interactButtonText;
    [Space]
    private Transform profileParent;
    private Image profileIcon;
    private Text raceText;
    private Text classText;

    [Header("Runtime Variables")]
    private OverworldTown currentTown; // The town currently opened by the player
    [Space]
    private Inventory inventory;
    private Player player;
    private readonly List<InventorySlot> equipablesSlots = new List<InventorySlot>();
    private readonly List<InventorySlot> consumablesSlots = new List<InventorySlot>();
    private readonly List<InventorySlot> sellSlots = new List<InventorySlot>();
    private readonly List<InventorySlot> equipmentSlots = new List<InventorySlot>();
    [Space]
    private int selectedIndex = 0;
    [Space]
    private int currentTab = 0;
    [Space]
    private AudioCore audioCore;

    [Header("Singleton")]
    public static TownManager instance;

    [Header("Test ITEM SO")]
    public List<ItemSO> EquipableTest;
    public List<ItemSO> ConsumableTest;

    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        player = DungeonManager.instance.player;

        for (int i = 0; i < equipablesBox.childCount; i++)
        {
            Image image = equipablesBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = equipablesBox.GetChild(i).GetChild(1).GetComponent<Text>();

            equipablesSlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        for (int i = 0; i < consumablesBox.childCount; i++)
        {
            Image image = consumablesBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = consumablesBox.GetChild(i).GetChild(1).GetComponent<Text>();

            consumablesSlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        for (int i = 0; i < sellBox.childCount; i++)
        {
            Image image = sellBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = sellBox.GetChild(i).GetChild(1).GetComponent<Text>();

            sellSlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        for (int i = 0; i < equipmentBox.childCount; i++)
        {
            Image image = equipmentBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = equipmentBox.GetChild(i).GetChild(1).GetComponent<Text>();

            equipmentSlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        interactButtonText = interactButton.transform.GetChild(0).GetComponent<Text>();

        profileParent = equipmentBox.parent;
        profileIcon = profileParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        raceText = profileParent.GetChild(1).GetComponent<Text>();
        classText = profileParent.GetChild(2).GetComponent<Text>();

        healthText = playerStatBox.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = playerStatBox.GetChild(1).GetChild(0).GetComponent<Text>();
        defenseText = playerStatBox.GetChild(2).GetChild(0).GetComponent<Text>();
        moneyText = playerStatBox.GetChild(3).GetComponent<Text>();

        audioCore = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioCore>();
    }

    public void EnterTown(OverworldTown town)
    {
        currentTown = town;
        ShowUI(0);
    }

    public void ShowUI(int tab)
    {
        UpdateProfileUI();
        UpdateEquipmentUI();
        player.UpdateTownStats();

        OpenTab(tab);

        townCanvas.SetActive(true);
    }

    public void UpdateEquipablesUI()
    {
        foreach (InventorySlot slot in equipablesSlots)
            slot.ClearSlot();

        for (int i = 0; i < currentTown.equipablesAvailableList.Count; i++)
        {
            Item item = currentTown.equipablesAvailableList[i];

            if (item == null) continue;

            equipablesSlots[i].SetSlot(item);
        }

        if (currentTown.equipablesAvailableList.Count == 0)
        {
            selectionMarker.SetActive(false);
            SetItemStats(false);
        }

        else
            SelectItem(selectedIndex);
    }

    public void UpdateConsumablesUI()
    {
        foreach (InventorySlot slot in consumablesSlots)
            slot.ClearSlot();

        for (int i = 0; i < currentTown.consumablesAvailableList.Count; i++)
        {
            Item item = currentTown.consumablesAvailableList[i];

            if (item == null) continue;

            consumablesSlots[i].SetSlot(item);
        }

        if (currentTown.consumablesAvailableList.Count == 0)
        {
            selectionMarker.SetActive(false);
            SetItemStats(false);
        }

        SelectItem(selectedIndex);
    }

    public void UpdateSellUI()
    {
        foreach (InventorySlot slot in sellSlots)
            slot.ClearSlot();

        for (int i = 0; i < inventory.items.Length; i++)
        {
            Item item = inventory.items[i];

            if (item == null)
                continue;

            sellSlots[i].SetSlot(item);
        }

        if (inventory.IsEmpty())
        {
            selectionMarker.SetActive(false);
            SetItemStats(false);
        }

        else
            SelectItem(selectedIndex);
    }

    public void UpdateProfileUI()
    {
        profileIcon.sprite = player.baseStat.racePortrait;

        raceText.text = player.baseStat.entityName;
        classText.text = "Lv. " + AbilityTree.instance.playerLevel + " " + player.classStat.className;
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

    public void OpenTab(int index)
    {
        currentTab = index;
        /*
         * 0 is Equipables
         * 1 is Consumables
         * 2 is Sell
         */

        if (index == 0) // EQUIPABLES
        {
            UpdateEquipablesUI();

            equipablesTabButton.interactable = false;
            consumablesTabButton.interactable = true;
            sellTabButton.interactable = true;

            if (currentTown.equipablesAvailableList.Count > 0)
                SelectItem(0);

            equipablesBox.gameObject.SetActive(true);
            consumablesBox.gameObject.SetActive(false);
            sellBox.gameObject.SetActive(false);

            interactButtonText.text = "BUY";
        }

        if (index == 1) // CONSUMABLES
        {
            UpdateConsumablesUI();

            equipablesTabButton.interactable = true;
            consumablesTabButton.interactable = false;
            sellTabButton.interactable = true;

            if (currentTown.consumablesAvailableList.Count > 0)
                SelectItem(0);

            equipablesBox.gameObject.SetActive(false);
            consumablesBox.gameObject.SetActive(true);
            sellBox.gameObject.SetActive(false);

            interactButtonText.text = "BUY";
        }

        if (index == 2) // SELL
        {
            UpdateSellUI();

            equipablesTabButton.interactable = true;
            consumablesTabButton.interactable = true;
            sellTabButton.interactable = false;

            SelectItem(0);

            equipablesBox.gameObject.SetActive(false);
            consumablesBox.gameObject.SetActive(false);
            sellBox.gameObject.SetActive(true);

            interactButtonText.text = "SELL";
        }

        audioCore.PlaySFX("SELECT");
    }

    public void CloseTown()
    {
        townCanvas.SetActive(false);
        OverworldManager.instance.SetAllowSelection(true);
    }
    public void UpdatePlayerStats(int currentHealth, int maxHealth, int defense, int damage)
    {
        healthText.text = currentHealth + "/" + maxHealth + " HP";
        attackText.text = damage + " ATK";
        defenseText.text = defense + " DEF";


        TooltipData healthData = new TooltipData()
        {
            header = "    HEALTH",
            headerIcon = healthText.transform.parent.GetComponent<Image>().sprite,
            leftHeader = "CURRENT STAT: " + currentHealth + "/" + maxHealth,
            description = "Health is used to keep track of how much health you have and when you lose health the health number drops a little. If you have no health you lose gaming."
        };
        healthText.transform.parent.GetComponent<Hoverable>().SetInformation(healthData);

        TooltipData attackData = new TooltipData()
        {
            header = "    ATTACK",
            headerIcon = attackText.transform.parent.GetComponent<Image>().sprite,
            leftHeader = "CURRENT STAT: " + damage,
            description = "Attack is used when attacking an enemy using your equipped weapon. Weapons that deal splash damage deal 50% of the Attack Stat"
        };
        attackText.transform.parent.GetComponent<Hoverable>().SetInformation(attackData);

        int defensePercentage = (int)(100 - 100 * (1 - defense / (float)(36 + defense)));
        TooltipData defenseData = new TooltipData()
        {
            header = "    DEFENSE",
            headerIcon = defenseText.transform.parent.GetComponent<Image>().sprite,
            leftHeader = "CURRENT STAT: " + defense,
            description = "Defense is used to reduce a percentage of all damage taken.\nYou are currently blocking " + defensePercentage + "% of all damage taken."
        };
        defenseText.transform.parent.GetComponent<Hoverable>().SetInformation(defenseData);

        moneyText.text = "";
    }


    public void SelectItem(int index)
    {
        selectedIndex = index;

        switch (currentTab)
        {
            case 0: // Equippables

                if (currentTown.equipablesAvailableList.Count <= index)
                {
                    selectedIndex--;
                    SelectItem(selectedIndex);
                    return;
                }

                equipablesSlots[index].SelectSlot();
                ShowItemInfo(currentTown.equipablesAvailableList[index]);
                break;
            

            case 1: // Consumables

                if (currentTown.consumablesAvailableList.Count <= index)
                {
                    selectedIndex--;
                    SelectItem(selectedIndex);
                    return;
                }

                consumablesSlots[index].SelectSlot();
                ShowItemInfo(currentTown.consumablesAvailableList[index]);
                break;


            case 2: // Sell

                if (inventory.IsEmpty())
                    return;

                if (inventory.Count() <= index)
                {
                    selectedIndex--;
                    SelectItem(selectedIndex);
                    return;
                }

                sellSlots[index].SelectSlot();
                ShowItemInfo(inventory.items[index]);
                break;
        }

        selectionMarker.SetActive(true);
        SetItemStats(true);

        audioCore.PlaySFX("SELECT");
    }

    private void ShowItemInfo(Item item)
    {
        itemStatParent.SetActive(false);

        foreach (Transform tr in itemStatParent.transform)
            tr.gameObject.SetActive(false);

        if (item == null)
        {
            itemIcon.transform.parent.gameObject.SetActive(false);
            itemNameText.text = "";
            itemTypeText.text = "";
            itemDescriptionText.text = "";

            interactButtonText.text = ":)";
            interactButton.interactable = false;
            // Clear all data
            return;
        }

        itemIcon.transform.parent.gameObject.SetActive(true);

        itemIcon.sprite = item.itemSprite;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;

        int rowIndex = 0;
        switch (item.itemType)
        {
            case ItemType.WEAPON:
                itemTypeText.text = "Weapon";
                WeaponSO weaponData = ((Weapon)item).weaponData;
                if (weaponData.weaponDamage != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("ATK");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = ((Weapon)item).weaponDamage + " ATK";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }
                if (weaponData.bonusDefense != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("DEF");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = ((Weapon)item).weaponData.bonusDefense + " DEF";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }
                if (weaponData.bonusMP != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("MP");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = ((Weapon)item).weaponData.bonusMP + " MP";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }

                itemStatParent.SetActive(true);
                break;

            case ItemType.ARMOR:
                itemTypeText.text = "Armor";
                Armor armor = (Armor)item;
                if (armor.health != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("HP");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = armor.health + " HP";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }
                if (armor.damage != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("ATK");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = armor.damage + " ATK";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }
                if (armor.defense != 0)
                {
                    itemStatParent.transform.GetChild(rowIndex).GetComponent<Image>().sprite = IconManager.instance.GetSprite("DEF");
                    itemStatParent.transform.GetChild(rowIndex).GetChild(0).GetComponent<Text>().text = armor.defense + " DEF";
                    itemStatParent.transform.GetChild(rowIndex).gameObject.SetActive(true);
                    rowIndex++;
                }

                itemStatParent.SetActive(true);
                break;

            case ItemType.ACCESSORY:
                itemTypeText.text = "Accessory";
                break;

            case ItemType.CONSUMABLE:
                itemTypeText.text = "Consumable";
                itemStatParent.transform.GetChild(0).GetComponent<Image>().sprite = IconManager.instance.GetSprite("HP");
                itemStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Consumable)item).consumableValue + " HP";
                itemStatParent.transform.GetChild(0).gameObject.SetActive(true);
                itemStatParent.SetActive(true);
                break;

            case ItemType.THROWABLE:
                itemTypeText.text = "Throwable";
                itemStatParent.transform.GetChild(0).GetComponent<Image>().sprite = IconManager.instance.GetSprite("ATK");
                itemStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Throwable)item).throwableValue + " ATK";
                itemStatParent.transform.GetChild(0).gameObject.SetActive(true);
                itemStatParent.SetActive(true);
                break;
        }
    }

    public void Interact()
    {
        if (currentTab == 2)
            Sell();
        else
            Buy();
    }

    private void Buy()
    {
        if (inventory.IsFull())
            return;


        if (currentTab == 0)
        {
            if (currentTown.equipablesAvailableList.Count <= selectedIndex)
                return;

            // if (enough money)
            Item itemToAdd = currentTown.equipablesAvailableList[selectedIndex];
            Item itemCopy = Inventory.CreateItem(itemToAdd.data, 1);

            inventory.AddItem(itemCopy);
            currentTown.equipablesAvailableList.RemoveAt(selectedIndex);

            UpdateEquipablesUI();

            audioCore.PlaySFX("SELECT");
        }

        else
        {
            if (currentTown.consumablesAvailableList.Count <= selectedIndex)
                return;

            // if (enough money)
            Item itemToAdd = currentTown.consumablesAvailableList[selectedIndex];
            Item itemCopy = Inventory.CreateItem(itemToAdd.data, 1);

            inventory.AddItem(itemCopy);

            if (currentTown.consumablesAvailableList[selectedIndex].count == 1)
                currentTown.consumablesAvailableList.RemoveAt(selectedIndex);
            else
                currentTown.consumablesAvailableList[selectedIndex].count--;

            UpdateConsumablesUI();

            audioCore.PlaySFX("SELECT");
        }
    }

    private void Sell()
    {
        if (inventory.items.Length <= selectedIndex)
            return;

        if (inventory.items[selectedIndex] == null)
            return;


        Item itemToAdd = inventory.items[selectedIndex];
        Item itemCopy = Inventory.CreateItem(itemToAdd.data, 1);

        if (inventory.items[selectedIndex].itemType == ItemType.CONSUMABLE || inventory.items[selectedIndex].itemType == ItemType.THROWABLE)
            AddItem(currentTown.consumablesAvailableList, itemCopy);

        else
            AddItem(currentTown.equipablesAvailableList, itemCopy);

        // money++;
        inventory.RemoveItem(selectedIndex);

        UpdateSellUI();

        audioCore.PlaySFX("SELECT");
    }

    private void AddItem(List<Item> items, Item itemToAdd)
    {
        foreach(Item i in items)
        {
            if (i == null)
                continue;

            if (i.itemName == itemToAdd.itemName && i.count < i.maxStack)
            {
                i.count += itemToAdd.count;
                return;
            }
        }

        items.Add(itemToAdd);
    }

    private void SetItemStats(bool active)
    {
        itemIcon.gameObject.SetActive(active);
        itemNameText.gameObject.SetActive(active);
        itemTypeText.gameObject.SetActive(active);
        itemDescriptionText.gameObject.SetActive(active);
        itemStatParent.SetActive(active);

        interactButton.interactable = active;
    }
}