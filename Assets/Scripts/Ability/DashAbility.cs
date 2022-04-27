using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> dashableTiles = new List<Tile>();
    private Tile targetTile;
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (!dashableTiles.Contains(tile))
            return false;

        player.transform.position = tile.transform.position;

        targetTile = tile;

        return true;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
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
        throw new System.NotImplementedException();
    }
}
