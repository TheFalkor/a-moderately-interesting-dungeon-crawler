using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensing
{
    [Header("References")]
    Entity myself;
    Player player;
    TilePathfinding pathfinder;

    public EnemySensing(Entity myEntity)
    {
        myself = myEntity;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        pathfinder = new TilePathfinding();
    }

    public Queue<Direction> GetPathToPlayer()
    {
        return pathfinder.CreatePath(myself.GetTile(), player.GetTile());
    }

    public bool IsPlayerInLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(myself.transform.position, player.transform.position);

        if (hit.transform.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }
}
