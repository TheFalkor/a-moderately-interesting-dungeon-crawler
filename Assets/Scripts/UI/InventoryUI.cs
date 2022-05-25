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
    [Space]
    [SerializeField] private Button inventoryTabButton;
    [SerializeField] private Button abilityTabButton;
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
    [Space]
    [SerializeField] private GameObject weaponStatParent;
    [SerializeField] private GameObject armorStatParent;
    [SerializeField] private GameObject consumableStatParent;
    [SerializeField] private GameObject throwableStatParent;
    [SerializeField] private GameObject accessoryStatParent;
    [Space]
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
    private bool isSelectedClass = false;
    private int selectedAbilityIndex = 0;
    [Space]
    private int currentTab = 0;
    [Space]
    private AudioKor audioKor;

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

            inventorySlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        for (int i = 0; i < equipmentBox.childCount; i++)
        {
            Image image = equipmentBox.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = equipmentBox.GetChild(i).GetChild(1).GetComponent<Text>();

            equipmentSlots.Add(new InventorySlot(image, text, selectionMarker, true));
        }

        useButtonText = useButton.transform.GetChild(0).GetComponent<Text>();

        profileParent = equipmentBox.parent;
        profileIcon = profileParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        raceText = profileParent.GetChild(1).GetComponent<Text>();
        classText = profileParent.GetChild(2).GetComponent<Text>();

        healthText = playerStatBox.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = playerStatBox.GetChild(1).GetChild(0).GetComponent<Text>();
        defenseText = playerStatBox.GetChild(2).GetChild(0).GetComponent<Text>();
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

        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    public void ShowUI(int tab)
    {
        UpdateInventoryUI();
        UpdateProfileUI();
        player.UpdateInventoryStats();

        OpenTab(tab);

        inventoryCanvas.SetActive(true);
    }

    public void OpenTab(int index)
    {
        currentTab = index;
        if (index == 0)
        {
            inventoryTabButton.interactable = false;
            abilityTabButton.interactable = true;

            SelectItem(0);
            inventoryBox.gameObject.SetActive(true);
            abilityBox.gameObject.SetActive(false);

            skillPointText.gameObject.SetActive(false);
        }
        else
        {
            inventoryTabButton.interactable = true;
            abilityTabButton.interactable = false;

            ShowItemInfo(null);

            selectionMarker.SetActive(false);
            abilityBox.gameObject.SetActive(true);
            inventoryBox.gameObject.SetActive(false);
            LoadAbilityUI();

            skillPointText.gameObject.SetActive(true);

            audioKor.PlaySFX("SELECT");
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
        profileIcon.sprite = player.baseStat.racePortrait;

        raceText.text = player.baseStat.entityName;
        classText.text = "Lv. " + AbilityTree.instance.playerLevel + " " + player.classStat.className;
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
        xpText.text = "";
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

        audioKor.PlaySFX("SELECT");
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

        audioKor.PlaySFX("SELECT");
        ShowItemInfo(item, true);
    }

    public void SelectRaceAbility(int index)
    {
        selectedAbilityIndex = index;
        isSelectedClass = false;

        raceSlots[index].SelectSlot();
        audioKor.PlaySFX("SELECT");

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
        selectedAbilityIndex = index;
        isSelectedClass = true;
        classSlots[index].SelectSlot();
        audioKor.PlaySFX("SELECT");

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

            skillPointText.text = "Points Available: " + AbilityTree.instance.skillPoints;

            LoadAbilityUI();

            if (isSelectedClass)
                SelectClassAbility(selectedAbilityIndex);
            else
                SelectRaceAbility(selectedAbilityIndex);
        }
    }

    private void ShowItemInfo(Item item, bool disableButton = false)
    {
        weaponStatParent.SetActive(false);
        armorStatParent.SetActive(false);
        consumableStatParent.SetActive(false);
        throwableStatParent.SetActive(false);
        accessoryStatParent.SetActive(false);

        if (item == null)
        {
            itemIcon.transform.parent.gameObject.SetActive(false);
            itemNameText.text = "";
            itemTypeText.text = "";
            itemDescriptionText.text = "";

            useButtonText.text = ":)";
            useButton.interactable = false;
            // Clear all data
            return;
        }

        itemIcon.transform.parent.gameObject.SetActive(true);

        itemIcon.sprite = item.itemSprite;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;

        switch (item.itemType)
        {
            case ItemType.WEAPON:
                itemTypeText.text = "Weapon";
                weaponStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Weapon)item).weaponDamage + " ATK";
                weaponStatParent.SetActive(true);
                useButtonText.text = "EQUIP ITEM";
                selectedEquipment = true;
                break;

            case ItemType.ARMOR:
                itemTypeText.text = "Armor";
                armorStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Armor)item).health + " HP";
                armorStatParent.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ((Armor)item).damage + " ATK";
                armorStatParent.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = ((Armor)item).defense + " DEF";
                armorStatParent.SetActive(true);
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
                consumableStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Consumable)item).consumableValue + " HP";
                consumableStatParent.SetActive(true);
                useButtonText.text = "USE ITEM";
                selectedEquipment = false;
                break;

            case ItemType.THROWABLE:
                itemTypeText.text = "Throwable";
                throwableStatParent.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ((Throwable)item).throwableValue + " ATK";
                throwableStatParent.SetActive(true);
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
            raceSlots[raceIndex].SetSlot(ability, true);
            raceIndex++;
        }

        foreach (PassiveSO passive in player.baseStat.unlockablePassives)
        {
            raceSlots[raceIndex].SetSlot(passive, true);
            raceIndex++;
        }


        int classIndex = 0;
        foreach (PassiveSO passive in player.classStat.startingPassives)
        {
            classSlots[classIndex].SetSlot(passive, true);
            classIndex++;
        }

        foreach (AbilitySO ability in player.classStat.unlockableAbilities)
        {
            classSlots[classIndex].SetSlot(ability, true);
            classIndex++;
        }

        foreach (PassiveSO passive in player.classStat.unlockablePassives)
        {
            classSlots[classIndex].SetSlot(passive, true);
            classIndex++;
        }

        skillPointText.text = "Points Available: " + AbilityTree.instance.skillPoints;
    }

    public void UpdateEquippedUI()
    {
        if (inventory.equippedWeapon != null)
            CombatUI.instance.SetEquipmentData(inventory.equippedWeapon, 0);

        if (inventory.equippedArmor != null)
            CombatUI.instance.SetEquipmentData(inventory.equippedArmor, 1);

        if (inventory.equippedAccessory != null)
            CombatUI.instance.SetEquipmentData(inventory.equippedAccessory, 2);
    }
}
