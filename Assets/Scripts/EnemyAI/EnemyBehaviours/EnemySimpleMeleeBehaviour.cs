using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMeleeBehaviour : EnemyBehaviour
{
    private int actionPointMaximum;
    private int actionPoints;
    private int freeMoves;
    private int freeMovesMaximum;
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
        freeMovesMaximum = 1;
        actionPoints = actionPointMaximum;
        freeMoves = freeMovesMaximum;
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

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (distance == 1)
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, pathToPlayer.Peek()));
                actionPoints--;
            }

            else if (actionPoints == 1 && distance <= pebbleRange)
            {
                actionQueue.Enqueue(new Action(ActionType.PEBBLE, pathToPlayer.Peek()));
                actionPoints--;
            }

            else
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
        freeMoves = freeMovesMaximum;
        actionQueue.Clear();
        return;
    }
}
