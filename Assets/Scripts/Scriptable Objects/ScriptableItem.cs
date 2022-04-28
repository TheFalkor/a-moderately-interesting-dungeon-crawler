using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;//the inventory sprite of the item
    public int maxStackSize = 1;
    public int stackSize = 1;
    public virtual InventoryItem CreateItem() 
    {
        InventoryItem newItem = new InventoryItem();
        newItem.SetName(itemName);
        newItem.SetSprite(itemSprite);
        newItem.SetMaxStack(maxStackSize);
        newItem.SetStackAmount(stackSize);
        return newItem;
    }
}
    

