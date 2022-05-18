using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : EquippableItem
{
    AccessorySO data;

    public Accessory(AccessorySO data)
    {
        Initialize(data);

        this.data = data;
    }

    public override void OnEquip()
    {
        PassiveManager.instance.AddPassive(AbilityTree.instance.CreatePassive(data.passiveEffect));
    }

    public override void OnUnequip()
    {
        PassiveManager.instance.RemovePassive(data.passiveEffect);
    }
}
