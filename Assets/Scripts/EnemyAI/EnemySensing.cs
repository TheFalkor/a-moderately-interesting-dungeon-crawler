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

    public Queue<Direction> GetPathToPlayer()
    {
        return pathfinder.CreatePath(myself.currentTile, player.currentTile);
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
