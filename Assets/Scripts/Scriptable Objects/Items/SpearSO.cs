using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Weapon/Spear", fileName = "Default Spear", order = 2)]
public class SpearSO : WeaponSO
{
    [Range (0f, 1f)]
    public float splashDamageMultiplier;
    private void Reset()
    {
        itemType = ItemType.WEAPON;
        weaponType = WeaponType.SPEAR;
        splashDamageMultiplier = 0.5f;
    }
}
