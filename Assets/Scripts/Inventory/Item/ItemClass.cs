using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryItem 
{
    

    //varibles
    string itemName;
    ItemType typeOfItem;
    Sprite sprite;
    int maxStack = 1;
    int stackAmount = 1;
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
            Debug.Log("sprite is null");
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
    protected void SetType(ItemType type)
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
        //typeOfItem = itemToCopyFrom.GetItemType(); we only want to change type of item in constructor
        sprite = itemToCopyFrom.GetSprite();
        maxStack = itemToCopyFrom.maxStack;
        stackAmount = itemToCopyFrom.stackAmount;
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

    public bool IsSameItem(InventoryItem other) 
    {
        if (itemName == other.GetName())
        {
            return true;
        }
        return false;
    }
    public void SetMaxStack(int stackSize) 
    {
        maxStack = stackSize;
        if (maxStack < 1) 
        {
            maxStack = 1;
        }
    }
    public int GetStackAmount() 
    {
        return stackAmount;
    }
    public void SetStackAmount(int amount) 
    {
        stackAmount = amount;
    }
    public void CombineStacks(InventoryItem otherStack) 
    {
        
        if (!(stackAmount>=maxStack) && IsSameItem(otherStack)&&this!=otherStack) 
        {
            stackAmount += otherStack.GetStackAmount();
            int leftover = 0;
            if (stackAmount > maxStack) 
            {
               leftover = stackAmount - maxStack;
               stackAmount = maxStack;
            }
            otherStack.SetStackAmount(leftover);
        }
    }
    public  InventoryItem SplitStack(int sizeOfSplit) 
    {
        InventoryItem otherStack=Copy();
        int itemsLeft = stackAmount - sizeOfSplit;
        if (itemsLeft < 0) 
        {
            itemsLeft = 0;
        }
        otherStack.SetStackAmount(stackAmount - itemsLeft);
        stackAmount = itemsLeft;
        return otherStack;
    }

    public void DecreaseStack(int amount) 
    {
        SetStackAmount(stackAmount - amount);
        if (stackAmount <= 0) 
        {
            RemoveFromInventory();
        }
    }
    
}
 
