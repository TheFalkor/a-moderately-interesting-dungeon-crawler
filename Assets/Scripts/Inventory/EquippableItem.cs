using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippableItem : Item
{
    public virtual void OnUnequip()
    {
        Debug.Log("OnUnequip() not implemented.");
    }

    public virtual void OnEquip()
    {
        Debug.Log("OnEquip() not implemented.");
    }
}
