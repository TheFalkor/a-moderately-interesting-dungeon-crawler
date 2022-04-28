using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    List<InventoryItem> inventory = new List<InventoryItem>();
    Occupant owner = null;
    EquipmentInventory myEquipedItems;
    public void SetOwner(Occupant newOwner) 
    {
        owner = newOwner;
    }
    public void AddItem(InventoryItem itemToAdd) 
    {
        if (itemToAdd != null) 
        {
            CheckForStacks(itemToAdd);
            if (itemToAdd.GetStackAmount() > 0) 
            {
                itemToAdd.SetWhereItemIsStored(this);
                inventory.Add(itemToAdd);
            }
        }
        
    }
    public void UseItem(InventoryItem itemToUse) 
    {
        if (HasItem (itemToUse)) 
        {
            itemToUse.UseItem(owner);
        }
    }
    public void UseItem(int index) 
    {
        if (index >= 0 && index < inventory.Count) 
        {
            inventory[index].UseItem(owner);
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
            inventoryToMoveTo.AddItem(itemToMove);
            return true;
        }
        return false;
    }
    public bool CreateEquipmentInventory() 
    {
        if (myEquipedItems == null) 
        {
            myEquipedItems = new EquipmentInventory(this);
            return true;
        }
        return false;
    }

    public bool Equip(Equipment equipment) 
    {
        if (myEquipedItems != null) 
        {
            return myEquipedItems.Equip(equipment);
        }
        return false;
    }
    public int GetAmountOfItems() 
    {
        return inventory.Count;
    }
    public InventoryItem GetItem(int index) 
    {
        if (index >= 0 && index < inventory.Count) 
        {
            return inventory[index];
        }
        return null;
    }
    public InventoryItem GetEquipedItem(EquipmentType slot,int slotNr) 
    {
        if (myEquipedItems != null) 
        {
            return myEquipedItems.GetEquipedInSlot(slot,slotNr);
        }
        return null;
    }
    public int EquipedItemsStatValue(StatType stat) 
    {
        if (myEquipedItems != null) 
        {
            return myEquipedItems.GetStatModifier(stat);
        }
        return 0;
    }
    public bool HasEquipmentInventory() 
    {
        if (myEquipedItems != null) 
        {
            return true;
        }
        return false;
    }

    public WeaponType GetEquipedWeaponType() 
    {
        return myEquipedItems.GetEquipedWeaponType();
    }
    public void UnequipItem(EquipmentType slot, int slotNr) 
    {
        if (myEquipedItems!=null) 
        {
            myEquipedItems.Unequip(slot, slotNr);
        }
    }
    private void CheckForStacks(InventoryItem itemToCheck) 
    {
        foreach(InventoryItem item in inventory) 
        {
            if (item.IsSameItem(itemToCheck)) 
            {
                item.CombineStacks(itemToCheck);
            }
        }
    }
}

