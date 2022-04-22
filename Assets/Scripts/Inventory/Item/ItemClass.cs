using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryItem //: ScriptableObject
{
    

    //varibles
    string itemName;
    ItemType typeOfItem;
    Sprite sprite;
    protected Inventory wherItemIsStored;
    public InventoryItem()
    {
        SetName("This item is buged");
        SetType(ItemType.UNASIGNED);
    }
    public void SetSprite(Sprite newSprite) 
    {
        sprite = newSprite;
    }
    public Sprite GetSprite() 
    {
        if (sprite == null) 
        {
            Debug.Log("nill");
        }
        return sprite;
    }
    public void SetName(string newName)
    {
        itemName = newName;
    }
    public string GetName() 
    {
        return itemName;
    }
    public void SetType(ItemType type)
    {
        typeOfItem = type;
    }
    public ItemType GetItemType() 
    {
        return typeOfItem;
    }
    public virtual InventoryItem Copy()
    {
        InventoryItem copyOfItem = new InventoryItem();
        copyOfItem.CopyValues(this);
        return copyOfItem;
    }
    public void CopyValues(InventoryItem itemToCopyFrom) 
    {
        itemName = itemToCopyFrom.itemName;
        typeOfItem = itemToCopyFrom.typeOfItem;
        sprite = itemToCopyFrom.GetSprite();
    }

    public virtual void UseItem(Occupant user)
    {
        Debug.Log("that item does not have a use");
    }
    public void SetWhereItemIsStored(Inventory inventory) 
    {
        wherItemIsStored = inventory;
    }
    public void RemoveFromInventory() 
    {
        wherItemIsStored.RemoveItem(this);
    }
    public void MoveToInventory(Inventory inventoryToMoveTo) 
    {
        wherItemIsStored.MoveItemToOtherInventory(inventoryToMoveTo, this);
    }
}
 
