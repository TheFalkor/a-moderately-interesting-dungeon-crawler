using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Weapon/Sword", fileName = "Default Sword", order = 5)]
public class SwordSO : WeaponSO
{
    private void Reset()
    {
        itemType = ItemType.WEAPON;
        weaponType = WeaponType.SWORD;
    }
}
