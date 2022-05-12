using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBlast : Ability
{
    List<Tile> dirN = new List<Tile>();
    List<Tile> dirE = new List<Tile>();
    List<Tile> dirS = new List<Tile>();
    List<Tile> dirW = new List<Tile>();

    bool northIsOccupied = false;
    bool eastIsOccupied = false;
    bool southIsOccupied = false;
    bool westIsOccupied = false;

    private float animationTimer = 0;

    [Header("References")]
    private Player player;
    private List<Tile> target = new List<Tile>();

    public override void HighlightDecisions(Tile currentTile)
    {
        player = (Player)currentTile.GetOccupant();

        affectedEnemies.Clear();
        target.Clear();

        dirN.Clear();
        dirE.Clear();
        dirS.Clear();
        dirW.Clear();

        Tile centerN = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down * 2);
        Tile centerE = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right * 2);
        Tile centerS = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up * 2);
        Tile centerW = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left * 2);
        
        if (centerN)
        {
            dirN.Add(centerN);
            dirN.AddRange(centerN.orthogonalNeighbors);
            dirN.AddRange(centerN.diagonalNeighbors);
        }
        
        if (centerE)
        {
            dirE.Add(centerE);
            dirE.AddRange(centerE.orthogonalNeighbors);
            dirE.AddRange(centerE.diagonalNeighbors);
        }
        
        if (centerS)
        {
            dirS.Add(centerS);
            dirS.AddRange(centerS.orthogonalNeighbors);
            dirS.AddRange(centerS.diagonalNeighbors);
        }
        
        if (centerW)
        {
            dirW.Add(centerW);
            dirW.AddRange(centerW.orthogonalNeighbors);
            dirW.AddRange(centerW.diagonalNeighbors);
        }
        
        foreach (Tile t in currentTile.diagonalNeighbors)
        {
            dirN.Remove(t);
            dirE.Remove(t);
            dirS.Remove(t);
            dirW.Remove(t);
        }
        
        foreach (Tile t in dirN)
        {
            t.Highlight(HighlightType.ABILITY_TARGET);
            if (t.IsOccupied())
                northIsOccupied = true;
        }
        
        foreach (Tile t in dirE)
        {
            t.Highlight(HighlightType.ABILITY_TARGET);
            if (t.IsOccupied())
                eastIsOccupied = true;
        }
        
        foreach (Tile t in dirS)
        {
            t.Highlight(HighlightType.ABILITY_TARGET);
            if (t.IsOccupied())
                southIsOccupied = true;
        }
        
        foreach (Tile t in dirW)
        {
            t.Highlight(HighlightType.ABILITY_TARGET);
            if (t.IsOccupied())
                westIsOccupied = true;
        }
    }

    public override bool Tick(float deltaTime)
    {
        if (animationTimer == -99)
            return true;

        if (animationTimer > 0)
        {
            animationTimer -= deltaTime;

            Color updateColor = Color.white;
            updateColor.g = Mathf.Cos(Mathf.Abs(Mathf.PI * animationTimer * 2));
            updateColor.b = Mathf.Cos(Mathf.Abs(Mathf.PI * animationTimer * 2));

            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = updateColor;
        }

        else
        {
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

            foreach(Tile t in target)
            {
                if (t.IsOccupied())
                {
                    Occupant tOccupant = t.GetOccupant();

                    if (t.GetOccupant() is Entity)
                        affectedEnemies.Add((Entity)tOccupant);

                    tOccupant.TakeDamage(new Damage(data.abilityValue, DamageOrigin.FRIENDLY));
                }
            }

            animationTimer = -99;

            return true;
        }

        return false;
    }

    public override bool UseAbility(Tile tile)
    {
        if (northIsOccupied)
        {
            if (dirN.Contains(tile))
            {
                target.AddRange(dirN);
                animationTimer = 1f;
                return true;
            }
        }

        if (eastIsOccupied)
        {
            if (dirE.Contains(tile))
            {
                target.AddRange(dirE);
                animationTimer = 1f;
                return true;
            }
        }

        if (southIsOccupied)
        {
            if (dirS.Contains(tile))
            {
                target.AddRange(dirS);
                animationTimer = 1f;
                return true;
            }
        }

        if (westIsOccupied)
        {
            if (dirW.Contains(tile))
            {
                target.AddRange(dirW);
                animationTimer = 1f;
                return true;
            }
        }

        return false;
    }
}
