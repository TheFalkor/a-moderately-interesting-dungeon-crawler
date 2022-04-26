using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePathfinding
{
    private bool canFly = false;
    private Tile targetTile;

    public Queue<Direction> path = new Queue<Direction>();
    private List<Cell> cellsToCheck = new List<Cell>();
    private List<Cell> cellsChecked = new List<Cell>();
    private List<Cell> gizmoPath = new List<Cell>();


    private class Cell
    {
        public readonly Tile tileReference;

        public readonly Vector2Int gridPosition;
        private Vector2Int targetPosition;
        public int gCost;
        public readonly float fCost;
        public Cell parent;

        public Cell(Tile tile, int gC, Vector2Int tPos, Cell previous)
        {
            tileReference = tile;

            gridPosition = tile.GetPosition();
            targetPosition = tPos;
            gCost = gC;
            fCost = Vector2.Distance(gridPosition, targetPosition);
            parent = previous;
        }

        public float GetTotalCost()
        {
            return gCost + fCost;
        }

        public List<Tile> GetNeighborList()
        {
            return tileReference.orthogonalNeighbors;
        }
    }

    public Queue<Direction> CreatePath(Tile start,  Tile target)
    {
        targetTile = target;
        //canFly = flight;

        bool pathFound = false;

        path.Clear();
        cellsToCheck.Clear();
        cellsChecked.Clear();

        Cell startingCell = new Cell(start, 0, targetTile.GetPosition(), null);
        cellsToCheck.Add(startingCell);

        while(!pathFound)
        {
            Stack<Cell> newCells = ScanCell(GetCellToCheck());

            if (newCells.Peek().gridPosition == targetTile.GetPosition())
            {
                pathFound = true;
                recursiveCreatePath(newCells.Peek());
                break;
            }

            else
            {
                foreach (Cell c in newCells)
                {
                    bool found = false;

                    foreach (Cell checkedCell in cellsChecked)
                    {
                        if (c.gridPosition == checkedCell.gridPosition)
                        {
                            found = true;

                            if (c.gCost < checkedCell.gCost)
                            {
                                checkedCell.gCost = c.gCost;
                                checkedCell.parent = c.parent;
                            }
                        }
                    }

                    if(!found)
                    {
                        foreach (Cell openCell in cellsToCheck)
                        {
                            if (c.gridPosition == openCell.gridPosition)
                            {
                                found = true;

                                if (c.gCost < openCell.gCost)
                                {
                                    openCell.gCost = c.gCost;
                                    openCell.parent = c.parent;
                                }
                            }
                        }
                    }

                    if (!found)
                        cellsToCheck.Add(c);
                }
            }
        }

        return path;
    }

    private Stack<Cell> ScanCell(Cell c)
    {
        cellsChecked.Add(c);
        cellsToCheck.Remove(c);

        Stack<Cell> output = new Stack<Cell>();

        foreach (Tile t in c.GetNeighborList())
        {
            if (t.GetPosition() == targetTile.GetPosition())
            {
                Cell newCell = new Cell(t, c.gCost + 1, targetTile.GetPosition(), c);
                output.Push(newCell);
                return output;
            }

            else if (!t.IsOccupied())
            {
                if (t.IsWalkable() || canFly)
                {
                    Cell newCell = new Cell(t, c.gCost + 1, targetTile.GetPosition(), c);
                    output.Push(newCell);
                }
            }
        }

        return output;
    }

    private Cell GetCellToCheck()
    {
        Cell lowestCell = cellsToCheck[0];

        foreach (Cell c in cellsToCheck)
        {
            float cCost = c.GetTotalCost();
            float lowestCost = lowestCell.GetTotalCost();

            if (cCost < lowestCost)
                lowestCell = c;

            else if (cCost == lowestCost)
            {
                if (c.fCost < lowestCell.fCost)
                    lowestCell = c;

                else if (c.fCost == lowestCell.fCost)
                    if (c.gCost < lowestCell.gCost)
                        lowestCell = c;
            }
        }

        return lowestCell;
    }

    private void recursiveCreatePath(Cell targetCell)
    {
        if (targetCell.parent != null)
        {
            recursiveCreatePath(targetCell.parent);
            path.Enqueue(GetDirection(targetCell));
            gizmoPath.Add(targetCell);
        }

        else
        {
            gizmoPath.Add(targetCell);
        }

        return;
    }

    private Direction GetDirection(Cell c)
    {
        Vector2 input = c.gridPosition - c.parent.gridPosition;

        Debug.Log(c.gridPosition);

        if (input == Vector2.up)
            return Direction.SOUTH;

        else if (input == Vector2.right)
            return Direction.EAST;

        else if (input == Vector2.down)
            return Direction.NORTH;

        else if (input == Vector2.left)
            return Direction.WEST;

        else
        {
            Debug.LogError("getDirection of TilePathfinding broke send help");
            return Direction.NORTH;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        foreach (Cell c in cellsToCheck)
        {
            Gizmos.DrawSphere(c.tileReference.transform.position, 0.2f);

            if (c.parent != null)
            {
                Gizmos.DrawLine(c.tileReference.transform.position, c.parent.tileReference.transform.position);
            }
        }

        Gizmos.color = Color.blue;

        foreach (Cell c in cellsChecked)
        {
            Gizmos.DrawSphere(c.tileReference.transform.position, 0.2f);

            if (c.parent != null)
            {
                Gizmos.DrawLine(c.tileReference.transform.position, c.parent.tileReference.transform.position);
            }
        }

        Gizmos.color = Color.magenta;

        foreach (Cell c in gizmoPath)
        {
            Gizmos.DrawSphere(c.tileReference.transform.position, 0.2f);

            if (c.parent != null)
            {
                Gizmos.DrawLine(c.tileReference.transform.position, c.parent.tileReference.transform.position);
            }
        }

    }
}
