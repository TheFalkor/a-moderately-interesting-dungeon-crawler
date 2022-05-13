using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePulse : Ability
{
    [Header("Runtime Variables")]
    private HashSet<Tile> pulseAoe = new HashSet<Tile>();
    private float animationTimer = 0;

    [Header("References")]
    private Player player;

    public override void HighlightDecisions(Tile currentTile)
    {
        if (!player)
            player = (Player)currentTile.GetOccupant();

        pulseAoe.Clear();
        affectedEnemies.Clear();


        foreach (Tile orthogonal in currentTile.orthogonalNeighbors)
        {
            foreach (Tile t in orthogonal.orthogonalNeighbors)
                pulseAoe.Add(t);
            foreach (Tile t in orthogonal.diagonalNeighbors)
                pulseAoe.Add(t);
        }

        pulseAoe.Remove(currentTile);
            
        foreach (Tile t in pulseAoe)
        {
            t.Highlight(HighlightType.ABILITY_TARGET);
            
            if (t.IsOccupied())
            {
                if (t.GetOccupant() is Entity)
                    affectedEnemies.Add((Entity)t.GetOccupant());
            }
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
            updateColor.r = Mathf.Cos(Mathf.Abs(Mathf.PI * animationTimer * 2));
            updateColor.g = Mathf.Cos(Mathf.Abs(Mathf.PI * animationTimer * 2));

            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = updateColor;
        }

        else
        {
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

            foreach(Entity e in affectedEnemies)
                 e.AddStatusEffect(new StatusEffect(StatusType.SLOWED, 1));

            animationTimer = -99;

            return true;
        }

        return false;
    }

    public override bool UseAbility(Tile tile)
    {
        if (!pulseAoe.Contains(tile))
            return false;

        animationTimer = 1f;
        return true;
    }
}
