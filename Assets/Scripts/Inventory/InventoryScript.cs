using System.Collections;
using System.Collections.Generic;
using UnityEngine;


   public class Inventory
{
    List<InventoryItem> inventory = new List<InventoryItem>();
    public void AddItem(InventoryItem itemToAdd) 
    {
        inventory.Add(itemToAdd);
    }
    public void UseItem(InventoryItem itemToUse) 
    {
        if (HasItem (itemToUse)) 
        {
            itemToUse.UseItem();
        }
    }
    public void UseItem(int index) 
    {
        if (index >= 0 && index < inventory.Count) 
        {
            inventory[index].UseItem();
        }
    }
    public bool HasItem(InventoryItem itemToCheck)
    {
        return inventory.Contains(itemToCheck);
    }
    public bool RemoveItem(InventoryItem itemToRemove) 
    {
        return inventory.Remove(itemToRemove);
    }
    public bool MoveItemToOtherInventory(Inventory inventoryToMoveTo,InventoryItem itemToMove) 
    {
        if (RemoveItem(itemToMove)) 
        {
            AddItem(itemToMove);
            return true;
        }
        return false;
    }
}

