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
    private Vector2Int futurePlayerPosition;

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

            else if (actionPoints == 1 && distance <= pebbleRange && Senses.IsPlayerInLineOfSight(futurePlayerPosition))
            {
                actionQueue.Enqueue(new Action(ActionType.PEBBLE, pathToPlayer.Peek()));
                actionPoints--;
            }

            else
            {
                updateFuturePosition(pathToPlayer.Peek());
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
        futurePlayerPosition = Senses.myself.currentTile.GetPosition();
        actionQueue.Clear();
        return;
    }
    private void updateFuturePosition(Direction direction)
    {
        switch(direction)
        {
            case Direction.NORTH:
                futurePlayerPosition += Vector2Int.down;
                break;
            case Direction.EAST:
                futurePlayerPosition += Vector2Int.right;
                break;
            case Direction.SOUTH:
                futurePlayerPosition += Vector2Int.up;
                break;
            case Direction.WEST:
                futurePlayerPosition += Vector2Int.left;
                break;
        }

        return;
    }
}
