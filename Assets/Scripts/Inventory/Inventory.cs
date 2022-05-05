using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public Item[] items = new Item[16];
    public ItemSO potionTest;
    public ItemSO weaonTest;

    public Weapon equipedWeapon;


    private void Awake()
    {
        Item t = CreateItem(potionTest, 9);
        Item t2 = CreateItem(weaonTest, 9);
        AddItem(t);
        AddItem(t2);
    }

    void Start()
    {
        
    }

    public void UseItem(int index)
    {
        if (items[index] == null)
            return;

        items[index].UseItem();
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
            // Move everything left
        }

        // Update inventory ui
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
}
