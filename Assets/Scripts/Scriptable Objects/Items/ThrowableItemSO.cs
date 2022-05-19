using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Throwable", fileName = "Default Consumable", order = 4)]
public class ThrowableItemSO : ItemSO
{
    [Header("Consumable Data")]
    public int throwableValue;
    public ThrowableType throwableType;
    public GameObject projectilePrefab;


    private void Reset()
    {
        itemType = ItemType.THROWABLE;
        stackSize = StackSize.STACK_SIZE_32;
    }
}
