using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    private Vector2Int gridPosition = Vector2Int.zero;
    private bool isWalkable;
    private Occupant occupant;
    private List<Tile> neighborList = new List<Tile>();


    public void Initialize(Vector2Int pos, bool walkable /*Sprite SO*/)
    {
        gridPosition = pos;
        isWalkable = walkable;

        if (!walkable)
            GetComponent<SpriteRenderer>().color = Color.black;
        // randomize sprite
        // GetComponent<SpriteRenderer>().sprite = null;
    }
    
    public bool IsWalkable()
    {
        return isWalkable;
    }

    public bool IsOccupied()
    {
        return occupant != null;
    }

    public Vector2Int GetPosition()
    {
        return gridPosition;
    }

    public bool SetOccupant(Occupant occ)
    {
        if (occupant)
            return false;

        occupant = occ;

        return true;
    }

    public void AddNeighbor(Tile tile)
    {
        neighborList.Add(tile);
    }

    public bool AttackTile(Damage damage)
    {
        if (occupant)
        {
            occupant.TakeDamage(damage);

            return true;
        }

        return false;
    }

}