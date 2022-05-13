using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePulse : Ability
{
    [Header("Runtime Variables")]

    private HashSet<Tile> pulseAoe = new HashSet<Tile>();
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
        player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

        foreach(Entity e in affectedEnemies)
        {
            e.AddStatusEffect(new StatusEffect(StatusType.SLOWED, 1));

            GameObject temp = Object.Instantiate(data.abilityVFX[0], e.transform.position, Quaternion.identity);
            Object.Destroy(temp, 19 / 15f);
        }

        return true;
    }

    public override bool UseAbility(Tile tile)
    {
        if (!pulseAoe.Contains(tile))
            return false;

        return true;
    }
}
