using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutterDash : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> dashableTiles = new List<Tile>();
    private Tile targetTile;

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (!dashableTiles.Contains(tile))
            return false;

        affectedEnemies.Clear();

        Vector2Int direction = tile.GetPosition() - player.currentTile.GetPosition();
        int dashedTiles = (int)direction.magnitude;
        direction.x /= dashedTiles;
        direction.y /= dashedTiles;

        for (int i = 1; i < dashedTiles; i++)
        {
            Occupant occ = GridManager.instance.GetTile(player.currentTile.GetPosition() + direction * i).GetOccupant();
            if (occ && occ.originType == DamageOrigin.ENEMY)
                affectedEnemies.Add((Entity)occ);
        }


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
            player.transform.Rotate(new Vector3(0, 0, 1800 * deltaTime * player.transform.localScale.x));
        }
        else
        {
            player.currentTile.SetOccupant(null);
            player.currentTile = targetTile;
            player.currentTile.SetOccupant(player);

            player.transform.eulerAngles = new Vector3(0, 0, 0);
            player.UpdateLayerIndex();

            foreach (Occupant occupant in affectedEnemies)
            {
                occupant.AddStatusEffect(new StatusEffect(StatusType.FLUTTER, 1));
            }

            return true;
        }
        return false;
    }
}
