using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Accessory", fileName = "Default Accessory", order = 2)]
public class AccessorySO : ItemSO
{
    [Header("Accessory Data")]
    public PassiveSO passiveEffect;

    private void Reset()
    {
        itemType = ItemType.ACCESSORY;
    }
}
