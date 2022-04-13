using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Consumable",menuName ="ScriptableObjects/Item/Consumable" )]
public class ScriptableConsumable : ScriptableItem
{
    public override InventoryItem CreateItem()
    {
        InventoryItem temporaryItem=base.CreateItem();
        Consumable newConsumable = new Consumable();
        newConsumable.CopyValues(temporaryItem);

        return newConsumable;
    }
}
