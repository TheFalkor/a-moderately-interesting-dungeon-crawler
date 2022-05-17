using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDumbBehaviour : EnemyBehaviour
{
    private int actionPoints = 0;
    private int freeMoves = 0;
    private EnemySensing Senses;
    private Tile futurePosition;

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
        actionQueue.Clear();

        Queue<Tile> pathToPlayer = Senses.GetPathToPlayer();

        if (pathToPlayer.Count == 0)
            pathToPlayer = Senses.GetPathToClosestEnemy();

        while (actionPoints > 0)
        {
            if (futurePosition.orthogonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, Senses.player.currentTile));
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

