using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Consumable : InventoryItem 
{
    List<ConsumableEffectStruct> effects = new List<ConsumableEffectStruct>();
    public Consumable() 
    {
        SetType(ItemType.CONSUMABLE);
        SetName("unnamed potion");
    }
    public void AddEffectToConsumable(ConsumableEffectStruct effect) 
    {
        effects.Add(effect);
    }
    void ApplyEffect(Occupant user,ConsumableEffectStruct effect )  
    {
        switch (effect.myEffect) 
        {
            case ConsumableEffect.HEALING:user.Heal(effect.value); break;
        }
    }
    void ApplyAllEffects(Occupant user) 
    {
        foreach(ConsumableEffectStruct effect in effects) 
        {
            ApplyEffect(user, effect);
        }
    }
    public override void UseItem(Occupant user)
    {
        ApplyAllEffects(user);
        RemoveFromInventory();
    }
    public void CopyValues(Consumable itemToCopyFrom)
    {
        base.CopyValues(itemToCopyFrom);
    }
    public override InventoryItem Copy()
    {
    Consumable copy = new Consumable();
        copy.CopyValues(this);
        return copy;
    }

}
