using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "ScriptableObjects/Item/Equipment")]
public class ScriptableEquipment :ScriptableItem
{
    public EquipmentType typeOfEquipment;
    public WeaponType typeOfWeapon;
    public int bonusHealth;
    public int attack;
    public int defense;
    
    public override InventoryItem CreateItem()
    {
        InventoryItem temporary=  base.CreateItem();
        Equipment newEquipment = new Equipment();
        newEquipment.CopyValues(temporary);
        newEquipment.SetEquipmentType(typeOfEquipment);
        newEquipment.SetWeaponType(typeOfWeapon);
        newEquipment.SetModifier(StatType.MAX_HEALTH, bonusHealth);
        newEquipment.SetModifier(StatType.ATTACK, attack);
        newEquipment.SetModifier(StatType.DEFENSE, defense);
        return newEquipment;
    }
}
