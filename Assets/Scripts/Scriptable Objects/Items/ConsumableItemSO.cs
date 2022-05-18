using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Consumable", fileName = "Default Consumable", order = 3)]
public class ConsumableItemSO : ItemSO
{
    [Header("Consumable Data")]
    public int consumableValue;
    public ConsumableType consumableType;
    

    private void Reset()
    {
        itemType = ItemType.CONSUMABLE;
        stackSize = StackSize.STACK_SIZE_32;
    }
}