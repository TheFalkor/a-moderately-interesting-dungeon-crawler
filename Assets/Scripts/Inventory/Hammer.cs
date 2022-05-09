using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    private float splashMultiplier;

    [Header("Runtime Variables")]
    private List<AoeSplash> availableAoes = new List<AoeSplash>();

    public Hammer(HammerSO data)
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
        Tile north = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile northWest = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down + Vector2Int.left);
        Tile northEast = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down + Vector2Int.right);

        if (north)
        {
            List<Tile> splashList = new List<Tile>();

            north.Highlight(HighlightType.ATTACKABLE);

            if (northWest)
            {
                splashList.Add(northWest);
                northWest.Highlight(HighlightType.SPLASH);
            }

            if (northEast)
            {
                splashList.Add(northEast);
                northEast.Highlight(HighlightType.SPLASH);
            }

            availableAoes.Add(new AoeSplash(north, splashList));
        }

        // SOUTH
        Tile south = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile southWest = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up + Vector2Int.left);
        Tile southEast = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up + Vector2Int.right);

        if (south)
        {
            List<Tile> splashList = new List<Tile>();

            south.Highlight(HighlightType.ATTACKABLE);

            if (southWest)
            {
                splashList.Add(southWest);
                southWest.Highlight(HighlightType.SPLASH);
            }

            if (southEast)
            {
                splashList.Add(southEast);
                southEast.Highlight(HighlightType.SPLASH);
            }

            availableAoes.Add(new AoeSplash(south, splashList));
        }

        // EAST
        Tile east = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);

        if (east)
        {
            List<Tile> splashList = new List<Tile>();

            east.Highlight(HighlightType.ATTACKABLE);

            if (northEast)
            {
                splashList.Add(northEast);
                northEast.Highlight(HighlightType.SPLASH);
            }

            if (southEast)
            {
                splashList.Add(southEast);
                southEast.Highlight(HighlightType.SPLASH);
            }

            availableAoes.Add(new AoeSplash(east, splashList));
        }


        // WEST
        Tile west = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left);

        if (west)
        {
            List<Tile> splashList = new List<Tile>();
            
            west.Highlight(HighlightType.ATTACKABLE);

            if (northWest)
            {
                splashList.Add(northWest);
                northWest.Highlight(HighlightType.SPLASH);
            }

            if (southWest)
            {
                splashList.Add(southWest);
                southWest.Highlight(HighlightType.SPLASH);
            }

            availableAoes.Add(new AoeSplash(west, splashList));
        }
    }

    public override void ExtraHighlight(Tile currentTile)
    {
        availableAoes.Clear();

        // NORTH
        Tile north = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        Tile northWest = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down + Vector2Int.left);
        Tile northEast = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down + Vector2Int.right);

        if (north)
            if (north.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();

                north.Highlight(HighlightType.ATTACKABLE);

                if (northWest)
                {
                    splashList.Add(northWest);
                    northWest.Highlight(HighlightType.SPLASH);
                }
                if (northEast)
                {
                    splashList.Add(northEast);
                    northEast.Highlight(HighlightType.SPLASH);
                }

                availableAoes.Add(new AoeSplash(north, splashList));
            }

        // SOUTH
        Tile south = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        Tile southWest = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up + Vector2Int.left);
        Tile southEast = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up + Vector2Int.right);

        if (south)
            if (south.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();

                south.Highlight(HighlightType.ATTACKABLE);

                if (southWest)
                {
                    splashList.Add(southWest);
                    southWest.Highlight(HighlightType.SPLASH);
                }

                if (southEast)
                {
                    splashList.Add(southEast);
                    southEast.Highlight(HighlightType.SPLASH);
                }

                availableAoes.Add(new AoeSplash(south, splashList));
            }

        // EAST
        Tile east = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);

        if (east)
            if (east.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();

                east.Highlight(HighlightType.ATTACKABLE);
                
                if (northEast)
                {
                    splashList.Add(northEast);
                    northEast.Highlight(HighlightType.SPLASH);
                }
                if (southEast)
                {
                    splashList.Add(southEast);
                    southEast.Highlight(HighlightType.SPLASH);
                }

                availableAoes.Add(new AoeSplash(east, splashList));
            }


        // WEST
        Tile west = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left);

        if (west)
            if (west.IsOccupied())
            {
                List<Tile> splashList = new List<Tile>();

                west.Highlight(HighlightType.ATTACKABLE);

                if (northWest)
                {
                    splashList.Add(northWest);
                    northWest.Highlight(HighlightType.SPLASH);
                }

                if (southWest)
                {
                    splashList.Add(southWest);
                    southWest.Highlight(HighlightType.SPLASH);
                }

                availableAoes.Add(new AoeSplash(west, splashList));
            }
    }

    private AoeSplash TileInAoe(Tile t)
    {
        foreach (AoeSplash aoe in availableAoes)
        {
            if (t == aoe.mainTile)
                return aoe;
            /*else if (aoe.splashTiles.Contains(t))
                return aoe;*/
        }

        return null;
    }

}
