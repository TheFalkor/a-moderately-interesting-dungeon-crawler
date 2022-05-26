using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : EquippableItem
{
    [Header("Weapon Data")]
    public int weaponDamage;
    public WeaponSO weaponData;
    public WeaponType weaponType;
    public GameObject attackVFX;

    public virtual List<WeaponStrike> Attack(Tile tile)
    {
        Debug.Log("Attack(Tile) not implemented.");
        return new List<WeaponStrike>();
    }

    public virtual void ExtraHighlight(Tile currentTile)
    {
        Debug.Log("ExtraHighlight(Tile currentTile) not implemented.");
        return;
    }

    public override void OnEquip()
    {
        DungeonManager.instance.player.RecalculateStats();
    }

    public abstract void CheckHighlights();
}
