using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Consumable",menuName ="ScriptableObjects/Item/Consumable" )]
public class ScriptableConsumable : ScriptableItem
{

    public List<ScriptableConsumableEffect> effects;
    public override InventoryItem CreateItem()
    {
        InventoryItem temporaryItem=base.CreateItem();
        Consumable newConsumable = new Consumable();
        newConsumable.CopyValues(temporaryItem);
        foreach(ScriptableConsumableEffect effect in effects) 
        {
            newConsumable.AddEffectToConsumable(effect.CreateEffect());
        }
        return newConsumable;
    }
}
