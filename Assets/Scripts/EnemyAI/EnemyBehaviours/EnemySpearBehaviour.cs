using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpearBehaviour : EnemyBehaviour
{
    private int actionPoints = 0;
    private int freeMoves = 0;
    private EnemySensing Senses;
    [SerializeField]
    private int pebbleRange = 1;
    private Tile futurePosition;
    private bool pathingToPlayer;

    private Queue<Action> actionQueue = new Queue<Action>();

    public override void Initialize(EnemySensing sensing)
    {
        Senses = sensing;
    }

    public override Queue<Action> DecideTurn(int ap, int mp, bool allowMovement = true)
    {
        actionPoints = ap;
        freeMoves = mp;
        futurePosition = Senses.myself.currentTile;
        pathingToPlayer = true;
        actionQueue.Clear();

        Queue<Tile> pathToPlayer = Senses.GetSpearPathToPlayer();

        if (pathToPlayer.Count == 0)
        {
            pathingToPlayer = false;
            pathToPlayer = Senses.GetPathToClosestEnemy();
        }

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (PlayerInMainTile())
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, Senses.player.currentTile));
                actionPoints--;
            }

            else if (futurePosition.orthogonalNeighbors.Contains(Senses.player.currentTile) && CanMoveAway() && allowMovement)
            {
                futurePosition = GridManager.instance.GetTile(futurePosition.GetPosition() - (Senses.player.currentTile.GetPosition() - futurePosition.GetPosition()));
                actionQueue.Enqueue(new Action(ActionType.MOVE, futurePosition));
                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }

            else if (futurePosition.orthogonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.SPLASH_ATTACK, Senses.player.currentTile));
                actionPoints--;
            }

            else if (actionPoints == 1 && freeMoves == 0 && distance <= pebbleRange && pathingToPlayer /*&& Senses.CheckLineOfSight(futurePlayerPosition.transform.position)*/)
            {
                actionQueue.Enqueue(new Action(ActionType.PEBBLE, Senses.player.currentTile));
                actionPoints--;
            }

            else if (pathToPlayer.Count > 0)
            {
                if (allowMovement && !pathToPlayer.Peek().IsOccupied())
                {
                    futurePosition = pathToPlayer.Peek();
                    actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));
                }

                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }

            else
                actionPoints = 0;
        }

        return actionQueue;
    }

    private bool PlayerInMainTile()
    {
        Vector2Int distance = Senses.player.currentTile.GetPosition() - futurePosition.GetPosition();

        if (distance == Vector2Int.down * 2)
            return true;
        else if (distance == Vector2Int.right * 2)
            return true;
        else if (distance == Vector2Int.up * 2)
            return true;
        else if (distance == Vector2Int.left * 2)
            return true;
        else
            return false;
    }

    private bool CanMoveAway()
    {
        Tile targetTile = GridManager.instance.GetTile(futurePosition.GetPosition() - (Senses.player.currentTile.GetPosition() - futurePosition.GetPosition()));

        if (targetTile)
            return !targetTile.IsOccupied() && targetTile.IsWalkable();
        else
            return false;
    }
}
