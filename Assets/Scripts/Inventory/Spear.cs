using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    private float splashMultiplier;

    [Header("Runtime Variables")]
    private List<AoeSplash> availableAoes = new List<AoeSplash>();

    public Spear(SpearSO data)
    {
        Initialize(data);

        weaponDamage = data.weaponDamage;
        weaponType = data.weaponType;
        splashMultiplier = data.splashDamageMultiplier;
    }

    public override List<WeaponStrike> Attack(Tile tile)
    {
        List<WeaponStrike> strikes = new List<WeaponStrike>();

        if (tile)
        {
            AoeSplash strikeAoE = TileInAoe(tile);

            if (strikeAoE != null)
            {
                strikes.Add(new WeaponStrike(new Damage(weaponDamage, DamageOrigin.FRIENDLY), 1f, strikeAoE.mainTile));

                foreach (Tile s in strikeAoE.splashTiles)
                    strikes.Add(new WeaponStrike(new Damage(weaponDamage, DamageOrigin.FRIENDLY), splashMultiplier, s));
            }
        }
            

        return strikes;
    }

    public override void HighlightDecision(Tile currentTile)
    {
        availableAoes.Clear();

        // NORTH
        Tile north1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile north2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down * 2);

        if (north1 && north2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(north1);
            availableAoes.Add(new AoeSplash(north2, splashList));

            north1.Highlight(HighlightType.SPLASH);
            north2.Highlight(HighlightType.ATTACKABLE);
        }


        // EAST
        Tile east1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);
        Tile east2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right * 2);

        if (east1 && east2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(east1);
            availableAoes.Add(new AoeSplash(east2, splashList));

            east1.Highlight(HighlightType.SPLASH);
            east2.Highlight(HighlightType.ATTACKABLE);
        }

        // SOUTH
        Tile south1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile south2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up * 2);

        if (south1 && south2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(south1);
            availableAoes.Add(new AoeSplash(south2, splashList));

            south1.Highlight(HighlightType.SPLASH);
            south2.Highlight(HighlightType.ATTACKABLE);
        }

        // WEST
        Tile west1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left);
        Tile west2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left * 2);

        if (west1 && west2)
        {
            List<Tile> splashList = new List<Tile>();
            splashList.Add(west1);
            availableAoes.Add(new AoeSplash(west2, splashList));

            west1.Highlight(HighlightType.SPLASH);
            west2.Highlight(HighlightType.ATTACKABLE);
        }
    }

    public override void ExtraHighlight(Tile currentTile)
    {
        availableAoes.Clear();

        // NORTH
        Tile north1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile north2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down * 2);

        if (north1 && north2)
            if (north1.IsOccupied() || north2.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();
                splashList.Add(north1);
                availableAoes.Add(new AoeSplash(north2, splashList));

                if (north1.IsOccupied())
                {
                    north1.Highlight(HighlightType.SPLASH);
                    north2.Highlight(HighlightType.ATTACKABLE);
                }

                if (north2.IsOccupied())
                    north2.Highlight(HighlightType.ATTACKABLE);
            }


        // EAST
        Tile east1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);
        Tile east2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right * 2);

        if (east1 && east2)
            if (east1.IsOccupied() || east2.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();
                splashList.Add(east1);
                availableAoes.Add(new AoeSplash(east2, splashList));

                if (east1.IsOccupied())
                {
                    east1.Highlight(HighlightType.SPLASH);
                    east2.Highlight(HighlightType.ATTACKABLE);
                }

                if (east2.IsOccupied())
                    east2.Highlight(HighlightType.ATTACKABLE);
            }

        // SOUTH
        Tile south1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile south2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up * 2);

        if (south1 && south2)
            if (south1.IsOccupied() || south2.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();
                splashList.Add(south1);
                availableAoes.Add(new AoeSplash(south2, splashList));

                if (south1.IsOccupied())
                {
                    south1.Highlight(HighlightType.SPLASH);
                    south2.Highlight(HighlightType.ATTACKABLE);
                }

                if (south2.IsOccupied())
                    south2.Highlight(HighlightType.ATTACKABLE);
            }

        // WEST
        Tile west1 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left);
        Tile west2 = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left * 2);

        if (west1 && west2)
            if (west1.IsOccupied() || west2.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();
                splashList.Add(west1);
                availableAoes.Add(new AoeSplash(west2, splashList));

                if (west1.IsOccupied())
                {
                    west1.Highlight(HighlightType.SPLASH);
                    west2.Highlight(HighlightType.ATTACKABLE);
                }

                if (west2.IsOccupied())
                    west2.Highlight(HighlightType.ATTACKABLE);
            }
    }

    private AoeSplash TileInAoe(Tile t)
    {
        foreach (AoeSplash aoe in availableAoes)
        {
            if (t == aoe.mainTile)
                return aoe;
            else if (aoe.splashTiles.Contains(t))
                return aoe;
        }

        return null;
    }
}
