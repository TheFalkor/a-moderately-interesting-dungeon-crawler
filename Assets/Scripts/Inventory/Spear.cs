using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    [Header("Runtime Variables")]
    private List<AoeSplash> availableAoes = new List<AoeSplash>();

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
            if (TileInAoe(tile))
                strikes.Add(new WeaponStrike(new Damage(weaponDamage, DamageOrigin.FRIENDLY), 1f, tile));

        return strikes;
    }

    public override void HighlightDecision(Tile currentTile)
    {
        availableAoes.Clear();

        // NORTH
        Tile north1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile north2 = GridManager.instance.GetTile(north1.GetPosition() + Vector2Int.down);

        if (north1 && north2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(north1);
            availableAoes.Add(new AoeSplash(north2, splashList));

            north1.Highlight(HighlightType.ATTACKABLE);
            north2.Highlight(HighlightType.ATTACKABLE);
        }


        // EAST
        Tile east1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);
        Tile east2 = GridManager.instance.GetTile(east1.GetPosition() + Vector2Int.right);

        if (east1 && east2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(east1);
            availableAoes.Add(new AoeSplash(east2, splashList));

            east1.Highlight(HighlightType.ATTACKABLE);
            east2.Highlight(HighlightType.ATTACKABLE);
        }

        // SOUTH
        Tile south1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile south2 = GridManager.instance.GetTile(south1.GetPosition() + Vector2Int.up);

        if (south1 && south2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(south1);
            availableAoes.Add(new AoeSplash(south2, splashList));

            south1.Highlight(HighlightType.ATTACKABLE);
            south2.Highlight(HighlightType.ATTACKABLE);
        }

        // WEST
        Tile west1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile west2 = GridManager.instance.GetTile(west1.GetPosition() + Vector2Int.down);

        if (west1 && west2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(west1);
            availableAoes.Add(new AoeSplash(west2, splashList));

            west1.Highlight(HighlightType.ATTACKABLE);
            west2.Highlight(HighlightType.ATTACKABLE);
        }
    }

    public override void ExtraHighlight(Tile currentTile)
    {
        // NORTH
        Tile north1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile north2 = GridManager.instance.GetTile(north1.GetPosition() + Vector2Int.down);

        if (north1 && north2)
            if (north1.IsOccupied() || north2.IsOccupied())
            {
                north1.Highlight(HighlightType.ATTACKABLE);
                north2.Highlight(HighlightType.ATTACKABLE);
            }


        // EAST
        Tile east1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);
        Tile east2 = GridManager.instance.GetTile(east1.GetPosition() + Vector2Int.right);

        if (east1 && east2)
            if (east1.IsOccupied() || east2.IsOccupied())
            {
                east1.Highlight(HighlightType.ATTACKABLE);
                east2.Highlight(HighlightType.ATTACKABLE);
            }

        // SOUTH
        Tile south1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile south2 = GridManager.instance.GetTile(south1.GetPosition() + Vector2Int.up);

        if (south1 && south2)
            if (south1.IsOccupied() || south2.IsOccupied())
            {
                south1.Highlight(HighlightType.ATTACKABLE);
                south2.Highlight(HighlightType.ATTACKABLE);
            }

        // WEST
        Tile west1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile west2 = GridManager.instance.GetTile(west1.GetPosition() + Vector2Int.down);

        if (west1 && west2)
            if (west1.IsOccupied() || west2.IsOccupied())
            {
                west1.Highlight(HighlightType.ATTACKABLE);
                west2.Highlight(HighlightType.ATTACKABLE);
            }
    }

    private bool TileInAoe(Tile t)
    {
        foreach (AoeSplash aoe in availableAoes)
        {
            if ((t = aoe.mainTile))
                return true;
        }

        return false;
    }
}
