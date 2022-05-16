using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Base Inventory")]
    [SerializeField] private List<ItemSO> startingItems = new List<ItemSO>();
    [Space]
    [SerializeField] private ItemSO startingWeapon;
    [SerializeField] private ItemSO startingArmor;
    [SerializeField] private ItemSO startingAccessory;

    [Header("Inventory")]
    [HideInInspector] public Item[] items = new Item[16];
    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public Armor equippedArmor;
    [HideInInspector] public Accessory equippedAccessory;


    private void Awake()
    {
    }

    private void Start()
    {
        if (startingWeapon)
        {
            AddItem(CreateItem(startingWeapon));
            EquipItem(0);
        }

        if (startingArmor)
        {
            AddItem(CreateItem(startingArmor));
            EquipItem(0);
        }

        if (startingAccessory)
        {
            AddItem(CreateItem(startingAccessory));
            EquipItem(0);
        }

        foreach (ItemSO data in startingItems)
        {
            Item t = CreateItem(data, 9);
            AddItem(t);
        }
        
    }

    public void UseItem(int index)
    {
        if (items[index] == null)
            return;

        items[index].UseItem();
        RemoveItem(index);
    }

    public int EquipItem(int index)
    {
        EquippableItem previousEquipped = null;
        int slotIndex = 0;

        ItemType type = items[index].itemType;

        switch (type)
        {
            case ItemType.WEAPON:
                previousEquipped = equippedWeapon;
                equippedWeapon = (Weapon)items[index];
                equippedWeapon.OnEquip();
                slotIndex = 0;
                break;
            case ItemType.ARMOR:
                previousEquipped = equippedArmor;
                equippedArmor = (Armor)items[index];
                equippedArmor.OnEquip();
                slotIndex = 1;
                break;

            case ItemType.ACCESSORY:
                previousEquipped = equippedAccessory;
                equippedAccessory = (Accessory)items[index];
                equippedAccessory.OnEquip();
                slotIndex = 2;
                break;
        }

        if (previousEquipped != null)
            previousEquipped.OnUnequip();


        items[index] = previousEquipped;

        if (items[index] == null)
        {
            OrganizeInventory();
            if (HotbarUI.instance)
                HotbarUI.instance.UpdateUI();
        }

        InventoryUI.instance.UpdateInventoryUI();
        InventoryUI.instance.UpdateEquipmentUI();

        return slotIndex;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                continue;

            if (items[i].itemName == item.itemName && items[i].count < items[i].maxStack)
            {
                items[i].count += item.count;

                if (items[i].count > items[i].maxStack)
                {
                    int leftoverCount = items[i].count - items[i].maxStack;
                    items[i].count = items[i].maxStack;

                    item.count = leftoverCount;
                    AddItem(item);
                }
                return;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return;
            }
        }
    }

    public void RemoveItem(int index)
    {

        items[index].count -= 1;

        if (items[index].count == 0)
        {
            items[index] = null;
            OrganizeInventory();
        }

        InventoryUI.instance.UpdateInventoryUI();
        if (HotbarUI.instance)
            HotbarUI.instance.UpdateUI();
    }

    public Item CreateItem(ItemSO data, int stackCount = 1)
    {
        Item item = null;

        switch (data.itemType)
        {
            case ItemType.WEAPON:
                WeaponType type = ((WeaponSO)data).weaponType;
                if (type == WeaponType.HAMMER)
                    item = new Hammer((HammerSO)data);
                else if (type == WeaponType.SPEAR)
                    item = new Spear((SpearSO)data);
                else if (type == WeaponType.SWORD)
                    item = new Sword((SwordSO)data);
                break;

            case ItemType.ARMOR:
                item = new Armor((ArmorSO)data);
                break;

            case ItemType.ACCESSORY: 
                break;

            case ItemType.CONSUMABLE: 
                item = new Consumable((ConsumableItemSO)data);
                item.count = stackCount;
                break;

            case ItemType.THROWABLE:
                break;
        }
        return item;
    }

    private void OrganizeInventory()
    {
        for (int i = 0; i < items.Length - 1; i++)
        {
            if (items[i] == null)
            {
                if (items[i + 1] == null)
                    break;
                items[i] = items[i + 1];
                items[i + 1] = null;
            }
        }
    }
}
