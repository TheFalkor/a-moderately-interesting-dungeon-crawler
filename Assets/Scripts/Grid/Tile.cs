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

    private SpriteRenderer render;
    [Header("Highlight Colors")]
    [SerializeField] private Color COLOR_WALKABLE;
    [SerializeField] private Color COLOR_ATTACKABLE;
    [SerializeField] private Color COLOR_SPLASH;
    [SerializeField] private Color COLOR_ABILITY;
    [SerializeField] private Color COLOR_HEALABLE;


    public void Initialize(Vector2Int pos)
    {
        gridPosition = pos;
        render = GetComponent<SpriteRenderer>();
    }

    public void Setup(bool wall)
    {
        isWalkable = !wall;

        if (isWalkable)
        {
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 7;
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

        switch (type)
        {
            case HighlightType.WALKABLE:
                if (!IsOccupied())
                    render.color = COLOR_WALKABLE;
                break;

            case HighlightType.ATTACKABLE:
                render.color = COLOR_ATTACKABLE;
                break;

            case HighlightType.SPLASH:
                render.color = COLOR_SPLASH;
                break;

            case HighlightType.ABILITY_TARGET:
                render.color = COLOR_ABILITY;
                break;

            case HighlightType.HEALABLE:
                render.color = COLOR_HEALABLE;
                break;
        }
    }

    public void ClearHighlight()
    {
        if (!isWalkable)
            return;

        render.color = Color.white;
    }

}