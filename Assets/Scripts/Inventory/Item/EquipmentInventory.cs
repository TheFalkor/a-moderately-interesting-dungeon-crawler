using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory 
{
    Equipment weaponSlot;
    Equipment armorSlot;
    const int maxAccessories = 2;
    Equipment[] accessoryslots=new Equipment[maxAccessories];
    int lastAddedAccesory = 0;
    Inventory connectedInventory;
    StatCollection combinedStatModifier=new StatCollection();

    public EquipmentInventory(Inventory parentInventory) 
    {
        connectedInventory = parentInventory;
    }
    public Equipment GetEquipedInSlot(EquipmentType slot, int slotNr) 
    {
        switch (slot)
        {
            case EquipmentType.WEAPON:return weaponSlot; 
            case EquipmentType.ARMOR: return armorSlot;
            case EquipmentType.ACCESSORY:
                {
                    if (slotNr >= 0 && slotNr < maxAccessories)
                    {
                        return accessoryslots[slotNr];
                    }
                    break;
                }
        }
        return null;
    }
    public bool Equip(Equipment equipment) 
    {
        bool returnValue = false;
        switch(equipment.GetEquipmentType()) 
        {
            case EquipmentType.WEAPON: returnValue = EquipWeapon(equipment); break;
            case EquipmentType.ARMOR: returnValue = EquipArmor(equipment); break;
            case EquipmentType.ACCESSORY: returnValue = EquipAccessory(equipment);break; 
        }
        CalculateCombinedStatModifier();
        return returnValue;
    }
    public void Unequip(EquipmentType slot,int slotNr) 
    {
        switch (slot)
        {
            case EquipmentType.WEAPON:UnequipWeapon(); break;
            case EquipmentType.ARMOR: UnequipArmor(); break;
            case EquipmentType.ACCESSORY:UnequipAccessory(slotNr); break;
        }
        CalculateCombinedStatModifier();
    }
    
    bool EquipWeapon(Equipment weapon) 
    {
        if (weaponSlot != null) 
        {
            UnequipWeapon();
        }
        if (connectedInventory.RemoveItem(weapon)) 
        {
            weaponSlot = weapon;
            return true;
        }
        return false;
    }


    void UnequipWeapon() 
    {
        connectedInventory.AddItem(weaponSlot);
        weaponSlot = null;
    }
    bool EquipArmor(Equipment armor)
    {
        if (armorSlot != null)
        {
            UnequipArmor();
        }
        if (connectedInventory.RemoveItem(armor))
        {
            armorSlot = armor;
            return true;
        }
        return false;
    }
    void UnequipArmor()
    {
        connectedInventory.AddItem(armorSlot);
        armorSlot = null;
    }
    bool EquipAccessory(Equipment accessory)
    {
        lastAddedAccesory++;
        if (lastAddedAccesory >= maxAccessories) 
        {
            lastAddedAccesory = 0;
        }

        if (accessoryslots[lastAddedAccesory] != null)
        {
            UnequipAccessory(lastAddedAccesory);
        }
        if (connectedInventory.RemoveItem(accessory))
        {
            accessoryslots[lastAddedAccesory] =accessory;
            return true;
        }
        return false;
    }
    void UnequipAccessory(int slotNr)
    {
        connectedInventory.AddItem(accessoryslots[slotNr]);
        accessoryslots[slotNr] = null;
    }

    void CalculateCombinedStatModifier() 
    {
        combinedStatModifier.AllStatsToZero();
        if (weaponSlot!=null) 
        {
            weaponSlot.AddStatModifiersTo(combinedStatModifier);
        }
        if (armorSlot != null) 
        {
            armorSlot.AddStatModifiersTo(combinedStatModifier);
        }
        
        for(int i = 0; i < maxAccessories; i++) 
        {
            if (accessoryslots[i] != null) 
            {
                accessoryslots[i].AddStatModifiersTo(combinedStatModifier);
            }
        }
    }

    public int GetStatModifier(StatType stat) 
    {
        return combinedStatModifier.GetStat(stat);
    }

    public void AddStatModifiersTo(StatCollection target) 
    {
        target.CombineStats(combinedStatModifier);
    }
}
