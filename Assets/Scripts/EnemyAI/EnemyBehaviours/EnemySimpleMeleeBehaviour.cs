using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMeleeBehaviour : EnemyBehaviour
{
    private int actionPointMaximum;
    [SerializeField]
    private int actionPoints;
    private int freeMoves;
    private EnemySensing Senses;
    [SerializeField]
    private int pebbleRange = 3;

    private Queue<Action> actionQueue;

    public EnemySimpleMeleeBehaviour(EnemySensing sensingSystem)
    {
        Senses = sensingSystem;
    }

    public override void Initialize()
    {
        actionPointMaximum = 2;
        actionPoints = actionPointMaximum;
        freeMoves = 1;
        actionQueue = new Queue<Action>();
    }

    public void DoAction()
    {
        // stuff
        return;
    }

    public override Queue<Action> DecideTurn()
    {
        ClearTurn();

        Queue<Direction> pathToPlayer = Senses.GetPathToPlayer();
        int distanceFromPlayer = pathToPlayer.Count;
        int maximumMoves = actionPoints + freeMoves;

        if (distanceFromPlayer < maximumMoves)
        {
            for (int i = 0; i < distanceFromPlayer; i++)
            {
                actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));

                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }

            Direction AttackDirection = pathToPlayer.Dequeue();

            for (int i = 0; i < actionPoints; i++)
            {
                actionQueue.Enqueue(new Action(ActionType.MELEEATTACK, AttackDirection));
            }

            actionPoints = 0;
        }

        else if (distanceFromPlayer >= maximumMoves && distanceFromPlayer < (maximumMoves + pebbleRange))
        {
            for (int i = 0; i < (maximumMoves - 1); i++)
            {
                actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));

                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }

            actionQueue.Enqueue(new Action(ActionType.PEBBLE, pathToPlayer.Dequeue()));
            actionPoints--;
        }

        else
        {
            for (int i = 0; i < maximumMoves; i++)
            {
                actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));

                if (freeMoves > 0)
                    freeMoves--;
                else
                    actionPoints--;
            }
        }

        return actionQueue;
    }

    private void ClearTurn()
    {
        actionPoints = actionPointMaximum;
        actionQueue.Clear();
        return;
    }
}
