using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    private Vector2Int gridPosition = Vector2Int.zero;
    private bool isWalkable = true;
    private Occupant occupant;
    private TileEffect tileEffect;
    [HideInInspector] public List<Tile> orthogonalNeighbors = new List<Tile>();
    [HideInInspector] public List<Tile> diagonalNeighbors = new List<Tile>();

    private SpriteRenderer highlightRender;
    private SpriteRenderer cornerRender;

    [Header("Highlight Colors")]
    [SerializeField] private Color COLOR_WALKABLE;
    [SerializeField] private Color COLOR_ATTACKABLE;
    [SerializeField] private Color COLOR_SPLASH;
    [SerializeField] private Color COLOR_ABILITY;
    [SerializeField] private Color COLOR_HEALABLE;


    public void Initialize(Vector2Int pos)
    {
        gridPosition = pos;
        highlightRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
        cornerRender = transform.GetChild(2).GetComponent<SpriteRenderer>();
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
        tileEffect = null;
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

    public void SetTileEffect(TileEffect effect)
    {
        if (tileEffect && effect != null)
            tileEffect.OnDespawn();

        tileEffect = effect;
    }

    public TileEffect GetTileEffect()
    {
        return tileEffect;
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

    public void Highlight(HighlightType type, bool allowOccupant = true)
    {
        if (isWalkable)
        {
            if (!(!allowOccupant && IsOccupied()))
                highlightRender.gameObject.SetActive(true);
        }

        cornerRender.gameObject.SetActive(true);

        switch (type)
        {
            case HighlightType.WALKABLE:
                if (!IsOccupied())
                {
                    highlightRender.color = COLOR_WALKABLE;
                    cornerRender.color = COLOR_WALKABLE;
                }
                break;

            case HighlightType.ATTACKABLE:
                highlightRender.color = COLOR_ATTACKABLE;
                cornerRender.color = COLOR_ATTACKABLE;
                break;

            case HighlightType.SPLASH:
                highlightRender.color = COLOR_SPLASH;
                cornerRender.color = COLOR_SPLASH;
                break;

            case HighlightType.ABILITY_TARGET:
                highlightRender.color = COLOR_ABILITY;
                cornerRender.color = COLOR_ABILITY;
                break;

            case HighlightType.HEALABLE:
                highlightRender.color = COLOR_HEALABLE;
                cornerRender.color = COLOR_HEALABLE;
                break;
        }

        Color color = cornerRender.color;
        if (isWalkable)
            color.a = 0.75f;
        else
            color.a = 0.5f;
        cornerRender.color = color;
    }

    public void ClearHighlight()
    {
        highlightRender.gameObject.SetActive(false); 
        cornerRender.gameObject.SetActive(false);
    }

}