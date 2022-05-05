using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    [Header("Runtime Variables")]
    private List<Tile> availableTiles = new List<Tile>();


    public Spear(SpearSO data)
    {
        Initialize(data);

        weaponDamage = data.weaponDamage;
        weaponType = data.weaponType;
    }

    public override List<WeaponStrike> Attack(Tile tile)
    {
        List<WeaponStrike> strikes = new List<WeaponStrike>();

        if (tile)
            if (availableTiles.Contains(tile))
                strikes.Add(new WeaponStrike(new Damage(weaponDamage, DamageOrigin.FRIENDLY), 1f, tile));

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
}
