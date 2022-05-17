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
    private Tile futurePlayerPosition;

    private Queue<Action> actionQueue = new Queue<Action>();

    public override void Initialize(EnemySensing sensing)
    {
        Senses = sensing;
    }

    public override Queue<Action> DecideTurn(int ap, int mp, bool allowMovement = true)
    {
        actionPoints = ap;
        freeMoves = mp;
        futurePlayerPosition = Senses.myself.currentTile;
        actionQueue.Clear();

        Queue<Tile> pathToPlayer = Senses.GetPathToPlayer();

        while (actionPoints > 0)
        {
            int distance = pathToPlayer.Count;

            if (futurePlayerPosition.orthogonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, pathToPlayer.Peek()));
                actionPoints--;
            }

            else if (futurePlayerPosition.diagonalNeighbors.Contains(Senses.player.currentTile))
            {
                actionQueue.Enqueue(new Action(ActionType.MELEE_ATTACK, Senses.player.currentTile));
                actionPoints--;
            }

            else if (actionPoints == 1 && freeMoves == 0 && distance <= pebbleRange /*&& Senses.CheckLineOfSight(futurePlayerPosition.transform.position)*/)
            {
                actionQueue.Enqueue(new Action(ActionType.PEBBLE, pathToPlayer.Peek()));
                actionPoints--;
            }

            else
            {
                if (allowMovement)
                {
                    futurePlayerPosition = pathToPlayer.Peek();
                    actionQueue.Enqueue(new Action(ActionType.MOVE, pathToPlayer.Dequeue()));
                }

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
        switch (direction)
        {
            case Direction.NORTH:
                futurePlayerPosition = GridManager.instance.GetTile(futurePlayerPosition.GetPosition() + Vector2Int.down);
                break;
            case Direction.EAST:
                futurePlayerPosition = GridManager.instance.GetTile(futurePlayerPosition.GetPosition() + Vector2Int.right);
                break;
            case Direction.SOUTH:
                futurePlayerPosition = GridManager.instance.GetTile(futurePlayerPosition.GetPosition() + Vector2Int.up);
                break;
            case Direction.WEST:
                futurePlayerPosition = GridManager.instance.GetTile(futurePlayerPosition.GetPosition() + Vector2Int.left);
                break;
        }

        return;
    }
}

