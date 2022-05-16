using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : EquippableItem
{
    public int health;
    public int defense;
    public int damage;

    public Armor(ArmorSO data)
    {
        Initialize(data);

        health = data.health;
        defense = data.defense;
        damage = data.damage;
    }

    public override void OnEquip()
    {

    }

    public override void OnUnequip()
    {

    }
}
