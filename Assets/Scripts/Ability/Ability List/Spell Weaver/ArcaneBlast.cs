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

    [Header("References")]
    private Player player;
    private List<Tile> target = null;

    public override void HighlightDecisions(Tile currentTile)
    {
        if (!player)
            player = (Player)currentTile.GetOccupant();

        affectedEnemies.Clear();
        target = null;

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

            GameObject temp = Object.Instantiate(data.abilityVFX[0], t.transform.position, Quaternion.identity);
            Object.Destroy(temp, 9 / 15f);
        }


        return true;
    }

    public override bool UseAbility(Tile tile)
    {
        if (northIsOccupied)
        {
            if (dirN.Contains(tile))
            {
                target = dirN;
                return true;
            }
        }

        if (eastIsOccupied)
        {
            if (dirE.Contains(tile))
            {
                target = dirE;
                return true;
            }
        }

        if (southIsOccupied)
        {
            if (dirS.Contains(tile))
            {
                target = dirS;
                return true;
            }
        }

        if (westIsOccupied)
        {
            if (dirW.Contains(tile))
            {
                target = dirW;
                return true;
            }
        }

        return false;
    }
}
