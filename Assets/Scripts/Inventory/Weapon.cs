using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : EquippableItem
{
    [Header("Weapon Data")]
    public int weaponDamage;
    public WeaponType weaponType;

    public virtual List<WeaponStrike> Attack(Tile tile)
    {
        Debug.Log("Attack(Tile) not implemented.");
        return new List<WeaponStrike>();
    }
}
