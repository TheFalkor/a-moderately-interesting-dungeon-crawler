using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMeleeBehaviour : EnemyBehaviour
{
    private int actionPoints = 0;
    private int freeMoves = 0;
    private EnemySensing Senses;
    [SerializeField]
    private int pebbleRange = 3;
    private Vector2Int futurePlayerPosition;

    private Queue<Action> actionQueue = new Queue<Action>();

    public override void Initialize(EnemySensing sensing)
    {
        Senses = sensing;
    }

    public override Queue<Action> DecideTurn(int ap, int mp)
    {
        actionPoints = ap;
        freeMoves = mp;
        futurePlayerPosition = Senses.myself.currentTile.GetPosition();
        actionQueue.Clear();

        Queue<Direction> pathToPlayer = Senses.GetPathToPlayer();

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (distance == 1)
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, pathToPlayer.Peek()));
                actionPoints--;
            }

            else if (actionPoints == 1 && freeMoves == 0 && distance <= pebbleRange && Senses.CheckLineOfSight(GridManager.instance.GetTile(futurePlayerPosition).transform.position))
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
