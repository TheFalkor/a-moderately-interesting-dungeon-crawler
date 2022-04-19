using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    private Vector2Int gridPosition = Vector2Int.zero;
    private bool isWalkable;
    private Occupant occupant;
    [HideInInspector] public List<Tile> orthogonalNeighbors = new List<Tile>();
    [HideInInspector] public List<Tile> diagonalNeighbors = new List<Tile>();


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
        return isWalkable && !occupant;
    }

    public bool IsOccupied()
    {
        return occupant != null;
    }

    public Vector2Int GetPosition()
    {
        return gridPosition;
    }

    public void SetOccupant(Occupant occ)
    {
        occupant = occ;
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

    public void Highlight(HighlightType type)
    {
        if (!isWalkable)
            return;

        if (type == HighlightType.ATTACKABLE)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.5f, 0.5f);
        }
        else if (!IsOccupied() && type == HighlightType.WALKABLE)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    public void ClearHighlight()
    {
        if (!isWalkable)
            return;

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

}