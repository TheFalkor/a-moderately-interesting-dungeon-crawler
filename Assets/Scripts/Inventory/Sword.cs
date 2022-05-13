using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [Header("Runtime Variables")]
    private List<Tile> availableTiles = new List<Tile>();

    public override void OnEquip()
    {
        DungeonManager.instance.player.ChangeMaxAP(1);
    }

    public override void OnUnequip()
    {
        DungeonManager.instance.player.ChangeMaxAP(-1);
    }

    public Sword(SwordSO data)
    {
        Initialize(data);

        weaponDamage = data.weaponDamage;
        weaponType = data.weaponType;
        attackVFX = data.attackVFX;
    }

    public override List<WeaponStrike> Attack(Tile tile)
    {
        List<WeaponStrike> strikes = new List<WeaponStrike>();

        if (tile)
            if (availableTiles.Contains(tile))
            {
                strikes.Add(new WeaponStrike(new Damage(weaponDamage, DamageOrigin.FRIENDLY), 1f, tile));
                GameObject vfx = Object.Instantiate(attackVFX, tile.transform.position, Quaternion.identity);
                Object.Destroy(vfx, 0.5f);
            }

        return strikes;
    }

    public override void HighlightDecision(Tile currentTile)
    {
        availableTiles.Clear();

        foreach (Tile tile in currentTile.orthogonalNeighbors)
        {
            availableTiles.Add(tile);
            tile.Highlight(HighlightType.ATTACKABLE);
        }

        foreach (Tile tile in currentTile.diagonalNeighbors)
        {
            availableTiles.Add(tile);
            tile.Highlight(HighlightType.ATTACKABLE);
        }
    }

    public override void ExtraHighlight(Tile currentTile)
    {
        availableTiles.Clear();

        foreach (Tile tile in currentTile.orthogonalNeighbors)
        {
            if (tile.IsOccupied())
            {
                availableTiles.Add(tile);
                tile.Highlight(HighlightType.ATTACKABLE);
            }
        }

        foreach (Tile tile in currentTile.diagonalNeighbors)
        {
            if (tile.IsOccupied())
            {
                availableTiles.Add(tile);
                tile.Highlight(HighlightType.ATTACKABLE);
            }
        }
    }
}
