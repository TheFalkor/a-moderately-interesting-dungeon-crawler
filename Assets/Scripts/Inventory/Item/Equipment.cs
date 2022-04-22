using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : InventoryItem
{
    EquipmentType typeOfEquipment;
    WeaponType typeOfWeapon;
    StatCollection statModifiers = new StatCollection();
    public Equipment() 
    {
        SetType(ItemType.EQUIPMENT);
        SetName("unamed equipment");
        typeOfEquipment = EquipmentType.UNASIGNED;
        typeOfWeapon = WeaponType.NONE;
    }
    public override void UseItem(Occupant user)
    {
        wherItemIsStored.Equip(this);
    }
    public override InventoryItem Copy()
    {
        Equipment copy = new Equipment();
        copy.CopyEquipmentValues(this);
        return copy;
    }
    public void SetEquipmentType(EquipmentType equipmentType) 
    {
        typeOfEquipment = equipmentType;
    }
    public EquipmentType GetEquipmentType() 
    {
        return typeOfEquipment;
    }

    public void SetWeaponType(WeaponType weapon) 
    {
        typeOfWeapon = weapon;
    }

    public WeaponType GetWeaponType() 
    {
        return typeOfWeapon;
    }
    public void CopyEquipmentValues(Equipment equipmentToCopyFrom) 
    {
        base.CopyValues(equipmentToCopyFrom);
        SetEquipmentType(equipmentToCopyFrom.GetEquipmentType());
    }
    public void SetModifier(StatType stat,int value) 
    {
        statModifiers.SetStat(stat, value);
    }

    public void GetModifier(StatType stat)
    {
        statModifiers.GetStat(stat);
    }

    public void AddStatModifiersTo(StatCollection target) 
    {
        target.CombineStats(statModifiers);
    }

}

