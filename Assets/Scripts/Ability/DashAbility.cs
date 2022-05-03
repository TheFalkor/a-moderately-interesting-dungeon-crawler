using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> dashableTiles = new List<Tile>();
    private Tile targetTile;
    private List<Occupant> victimList = new List<Occupant>();

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (!dashableTiles.Contains(tile))
            return false;

        if (tile.transform.position.x > player.transform.position.x)
            player.transform.localScale = new Vector3(-1, 1);
        else
            player.transform.localScale = new Vector3(1, 1);

        targetTile = tile;

        return true;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
        dashableTiles.Clear();
        victimList.Clear();

        player = (Player)currentTile.GetOccupant();

        Queue<Vector2Int> directionQueue = new Queue<Vector2Int>();

        directionQueue.Enqueue(new Vector2Int(0, -1));
        directionQueue.Enqueue(new Vector2Int(1, 0));
        directionQueue.Enqueue(new Vector2Int(0, 1));
        directionQueue.Enqueue(new Vector2Int(-1, 0));

        while (directionQueue.Count != 0)
        {
            Tile tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek());
            if (!tile || !tile.IsWalkable())
            {
                directionQueue.Dequeue();
                continue;
            }

            tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek() * 2);
            if (!tile || !tile.IsWalkable())
            {
                directionQueue.Dequeue();
                continue;
            }

            if (!tile.IsOccupied())
            {
                dashableTiles.Add(tile);
                tile.Highlight(HighlightType.ABILITY_TARGET);
            }

            tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek() * 3);
            if (!tile || !tile.IsWalkable() || tile.IsOccupied())
            {
                directionQueue.Dequeue();
                continue;
            }

            directionQueue.Dequeue();
            dashableTiles.Add(tile);

            tile.Highlight(HighlightType.ABILITY_TARGET);
        }
    }

    public override bool Tick(float deltaTime)
    {
        if (targetTile.transform.position != player.transform.position)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetTile.transform.position, deltaTime * 8);
        }
        else
        {
            player.currentTile.SetOccupant(null);
            player.currentTile = targetTile;
            player.currentTile.SetOccupant(player);

            player.UpdateLayerIndex();

            foreach (Occupant occupant in victimList)
            {
                occupant.TakeDamage(new Damage(2, DamageOrigin.FRIENDLY, null));
                //occupant.AddStatusEffect(new StatusEffect(StatusType.FLUTTER, 1));
            }

            return true;
        }
        return false;
    }
}
