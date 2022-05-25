using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : EquippableItem
{
    AccessorySO pData;

    public Accessory(AccessorySO data)
    {
        Initialize(data);

        this.pData = data;
    }

    public override void OnEquip()
    {
        PassiveManager.instance.AddPassive(AbilityTree.instance.CreatePassive(pData.passiveEffect));
    }

    public override void OnUnequip()
    {
        PassiveManager.instance.RemovePassive(pData.passiveEffect);
    }
}
