using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    private Vector2Int gridPosition = Vector2Int.zero;
    private bool isWalkable = true;
    private Occupant occupant;
    [HideInInspector] public List<Tile> orthogonalNeighbors = new List<Tile>();
    [HideInInspector] public List<Tile> diagonalNeighbors = new List<Tile>();


    public void Initialize(Vector2Int pos)
    {
        gridPosition = pos;
    }

    public void Setup(bool wall)
    {
        isWalkable = !wall;

        if (isWalkable)
        {
            gameObject.layer = 6;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            gameObject.layer = 7;
            //GetComponent<SpriteRenderer>().color = Color.black;
        }

        occupant = null;
    }

    public void UpdateTileset()
    {
        if (!isWalkable)
            TilesetManager.instance.CalculateWallTile(this);
        else
            TilesetManager.instance.CalculateFloorTile(this);
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

    public void SetOccupant(Occupant occ)
    {
        occupant = occ;
    }

    public Occupant GetOccupant()
    {
        return occupant;
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

        if (type == HighlightType.ABILITY_TARGET)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.75f, 1);
        }
        else if (type == HighlightType.ATTACKABLE)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.5f, 0.5f);
        }
        else if (type == HighlightType.WALKABLE)
        {
            if (IsOccupied())
                GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.5f, 0.5f);
            else
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