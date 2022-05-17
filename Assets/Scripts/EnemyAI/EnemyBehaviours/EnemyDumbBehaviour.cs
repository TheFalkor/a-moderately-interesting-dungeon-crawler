using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDumbBehaviour : EnemyBehaviour
{
    private int actionPoints = 0;
    private int freeMoves = 0;
    private EnemySensing Senses;

    private Queue<Action> actionQueue = new Queue<Action>();

    public override void Initialize(EnemySensing sensing)
    {
        Senses = sensing;
    }

    public override Queue<Action> DecideTurn(int ap, int mp, bool allowMovement = true)
    {
        actionPoints = ap;
        freeMoves = mp;
        actionQueue.Clear();

        Queue<Tile> pathToPlayer = Senses.GetPathToPlayer();

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (distance == 1)
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, pathToPlayer.Peek()));
                actionPoints--;
            }

            else
            {
                if (allowMovement)
                    actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));

                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }
        }

        return actionQueue;
    }
}

