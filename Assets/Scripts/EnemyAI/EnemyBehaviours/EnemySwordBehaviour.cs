using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordBehaviour : EnemyBehaviour
{
    private int actionPoints = 0;
    private int freeMoves = 0;
    private EnemySensing Senses;
    [SerializeField]
    private int pebbleRange = 3;
    private Tile futurePosition;
    private bool pathingToPlayer = true;

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

        Queue<Tile> pathToPlayer = Senses.GetPathToPlayer();

        if (pathToPlayer.Count == 0)
        {
            pathingToPlayer = false;
            pathToPlayer = Senses.GetPathToClosestEnemy();
        }

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (futurePosition.orthogonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, Senses.player.currentTile));
                actionPoints--;
            }

            else if (futurePosition.diagonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, Senses.player.currentTile));
                actionPoints--;
            }

            else if (actionPoints == 1 && freeMoves == 0 && distance <= pebbleRange && pathingToPlayer /*&& Senses.CheckLineOfSight(futurePlayerPosition.transform.position)*/)
            {
                actionQueue.Enqueue(new Action(ActionType.PEBBLE, pathToPlayer.Peek()));
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
}

