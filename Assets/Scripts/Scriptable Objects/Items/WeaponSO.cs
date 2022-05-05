using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSO : ItemSO
{
    [Header("Weapon Data")]
    public int weaponDamage;
    [HideInInspector] public WeaponType weaponType;
}
