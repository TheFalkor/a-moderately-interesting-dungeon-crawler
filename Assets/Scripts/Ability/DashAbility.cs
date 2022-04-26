using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [Header("Runtime Variables")]
    private Tile targetTile;


    public override bool UseAbility(Tile tile)
    {
        // if tile is not attackble for any reason
        // return false

        targetTile = tile;

        // Do attackie
        // return true

        return false;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
        Queue<Tile> tileQueue = new Queue<Tile>();
        List<Tile> jumpableTiles = new List<Tile>();

        tileQueue.Enqueue(GridManager.instance.GetTile(currentTile.GetPosition() + new Vector2Int(0, -3)));
        tileQueue.Enqueue(GridManager.instance.GetTile(currentTile.GetPosition() + new Vector2Int(3, 0)));
        tileQueue.Enqueue(GridManager.instance.GetTile(currentTile.GetPosition() + new Vector2Int(0, 3)));
        tileQueue.Enqueue(GridManager.instance.GetTile(currentTile.GetPosition() + new Vector2Int(-3, 0)));

        while (tileQueue.Count != 0)
        {
            if (!tileQueue.Peek() || !tileQueue.Peek().IsWalkable() || tileQueue.Peek().IsOccupied())
            {
                tileQueue.Dequeue();
                continue;
            }

            jumpableTiles.Add(tileQueue.Peek());
            tileQueue.Peek().Highlight(HighlightType.ABILITY_TARGET);
            tileQueue.Dequeue();
        }

        Debug.Log(jumpableTiles.Count);
        
    }

    public override bool Tick(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
