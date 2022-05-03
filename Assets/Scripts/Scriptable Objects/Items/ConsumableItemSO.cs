using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Consumable", fileName = "Default Consumable", order = 1)]
public class ConsumableItemSO : ItemSO
{
    [Header("Consumable Data")]
    public int consumableValue;
    public ConsumableType consumableType;
    

    private void Reset()
    {
        itemType = ItemType.CONSUMABLE;
    }
}
