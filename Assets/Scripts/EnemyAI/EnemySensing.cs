using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensing
{
    [Header("References")]
    public Entity myself;
    public Player player;
    private TilePathfinding pathfinder;
    private LayerMask wallsMask;

    public EnemySensing(Entity myEntity)
    {
        wallsMask = LayerMask.GetMask("Wall");
        myself = myEntity;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        pathfinder = new TilePathfinding();
    }

    public Queue<Tile> GetPathToPlayer()
    {
        return pathfinder.CreatePath(myself.currentTile, player.currentTile);
    }

    public Queue<Tile> GetPathToClosestEnemy()
    {
        Queue<Tile> path = new Queue<Tile>();
        float distanceFromPlayer = Vector2.Distance(myself.transform.position, player.transform.position);

        foreach (Entity e in CombatManager.instance.entityList)
        {
            if (e.originType != DamageOrigin.ENEMY)
                continue;

            if (e == myself)
                continue;

            if (Vector2.Distance(e.transform.position, player.transform.position) < distanceFromPlayer)
            {
                Queue<Tile> pathToEnemy = pathfinder.CreatePath(myself.currentTile, e.currentTile);

                if (pathToEnemy.Count > 0)
                {
                    path.Clear();
                    while (pathToEnemy.Count > 0)
                        path.Enqueue(pathToEnemy.Dequeue());

                    distanceFromPlayer = Vector2.Distance(e.transform.position, player.transform.position);
                }
            }
        }

        return path;
    }

    public Queue<Tile> GetSpearPathToPlayer()
    {
        Queue<Tile> output = GetPathToPlayer();
        Queue<Tile> temp = output;

        Tile north = GridManager.instance.GetTile(player.currentTile.GetPosition() + Vector2Int.down * 2);
        Tile east = GridManager.instance.GetTile(player.currentTile.GetPosition() + Vector2Int.right * 2);
        Tile south = GridManager.instance.GetTile(player.currentTile.GetPosition() + Vector2Int.up * 2);
        Tile west = GridManager.instance.GetTile(player.currentTile.GetPosition() + Vector2Int.left * 2);

        List<Tile> directions = new List<Tile>();

        if (north)
            directions.Add(north);
        if (east)
            directions.Add(east);
        if (south)
            directions.Add(south);
        if (west)
            directions.Add(west);

        foreach (Tile t in directions)
        {
            if (t.IsWalkable() && !t.IsOccupied())
                temp = pathfinder.CreatePath(myself.currentTile, t);

            if (output.Count == 0)
                output = temp;
            else if (temp.Count < output.Count && temp.Count > 0)
                output = temp;
        }

        return output;
    }

    public bool CheckLineOfSight(Vector3 position)
    {

        if (Physics2D.Linecast(position, player.transform.position, wallsMask))
        {
            return false;
        }

        return true;
    }
}
