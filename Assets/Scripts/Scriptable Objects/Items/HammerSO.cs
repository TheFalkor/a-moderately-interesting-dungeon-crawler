using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Weapon/Hammer", fileName = "Default Hammer", order = 5)]
public class HammerSO : WeaponSO
{
    [Range(0f, 1f)]
    public float splashDamageMultiplier;
    private void Reset()
    {
        itemType = ItemType.WEAPON;
        weaponType = WeaponType.HAMMER;
        splashDamageMultiplier = 0.5f;
    }
}