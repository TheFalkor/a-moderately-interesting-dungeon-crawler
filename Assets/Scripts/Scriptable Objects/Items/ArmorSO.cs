using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Armor", fileName = "Default Armor", order = 1)]
public class ArmorSO : ItemSO
{
    [Header("Armor Data")]
    public int health;
    public int defense;
    public int damage;

    public void Reset()
    {
        itemType = ItemType.ARMOR;
    }
}
