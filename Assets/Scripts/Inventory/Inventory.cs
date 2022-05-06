using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public Item[] items = new Item[16];
    [SerializeField] private List<ItemSO> startingItems = new List<ItemSO>();

    public Weapon equippedWeapon;
    public Armor equippedArmor;
    public Accessory equippedAccessory;


    private void Awake()
    {
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

    public void EquipItem(int index)
    {
        EquippableItem previousEquipped = null;

        ItemType type = items[index].itemType;

        switch (type)
        {
            case ItemType.WEAPON:
                previousEquipped = equippedWeapon;
                equippedWeapon = (Weapon)items[index];
                equippedWeapon.OnEquip();
                break;
            case ItemType.ARMOR:
                previousEquipped = equippedArmor;
                equippedArmor = (Armor)items[index];
                equippedArmor.OnEquip();
                break;

            case ItemType.ACCESSORY:
                previousEquipped = equippedAccessory;
                equippedAccessory = (Accessory)items[index];
                equippedAccessory.OnEquip();
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
                    item = null;
                else if (type == WeaponType.SPEAR)
                    item = null;
                else if (type == WeaponType.SWORD)
                    item = new Sword((SwordSO)data);
                break;

            case ItemType.ARMOR: 
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
