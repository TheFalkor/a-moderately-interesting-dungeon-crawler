using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSO : ItemSO
{
    [Header("Weapon Data")]
    public int weaponDamage;
    public int bonusDefense;
    public int bonusMP;
    [HideInInspector] public WeaponType weaponType;
    public GameObject attackVFX;
}
