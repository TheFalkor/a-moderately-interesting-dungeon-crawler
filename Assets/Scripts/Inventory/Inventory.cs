using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector] public Item[] items = new Item[16];
    public ConsumableItemSO potionTest;


    private void Awake()
    {
        Item t = CreateItem(potionTest);
        items[0] = t;
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

    }

    public void RemoveItem(int index)
    {

    }

    public Item CreateItem(ItemSO data)
    {
        switch (data.itemType)
        {
            case ItemType.WEAPON: 
                break;

            case ItemType.ARMOR: 
                break;

            case ItemType.ACCESSORY: 
                break;

            case ItemType.CONSUMABLE: 
                return new Consumable((ConsumableItemSO)data);

            case ItemType.THROWABLE:
                break;
        }
        return null;
    }
}
