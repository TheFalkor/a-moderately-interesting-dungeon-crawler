using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Consumable : InventoryItem 
    {
        public Consumable() 
        {
            SetType(ItemType.CONSUMABLE);
            SetName("unnamed potion");
        }
        public override void UseItem()
        {
            Debug.Log("drank a "+GetName());
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
