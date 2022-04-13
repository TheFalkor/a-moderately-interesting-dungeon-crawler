using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Player : MonoBehaviour
{
    private Entity entity;

    
    void Start()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()   // TEMPORARY: GameManager will call Tick() later ye
    {
        Tick(Time.deltaTime);
    }

    public bool Tick(float deltaTime)
    {
        if (entity.IsBusy())
            return false;


        HighlightDecision();
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                MoveToTile(hit.transform.GetComponent<Tile>());
            }
        }


        if (Input.GetKeyUp(KeyCode.W))
        {
            MoveToTile(GridManager.instance.GetTileWorld(transform.position + Vector3.up));
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            MoveToTile(GridManager.instance.GetTileWorld(transform.position + Vector3.left));
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            MoveToTile(GridManager.instance.GetTileWorld(transform.position + Vector3.down));
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            MoveToTile(GridManager.instance.GetTileWorld(transform.position + Vector3.right));
        }
        

        return false;
    }

    private void MoveToTile(Tile tile)
    {
        if (!tile || !tile.IsWalkable())
            return;

        if (!entity.currentTile.orthogonalNeighbors.Contains(tile))
            return;

        Vector2Int deltaPosition = entity.currentTile.GetPosition() - tile.GetPosition();
        Direction dir;

        if (deltaPosition.x < 0)
            dir = Direction.EAST;
        else if (deltaPosition.x > 0)
            dir = Direction.WEST;
        else if (deltaPosition.y < 0)
            dir = Direction.SOUTH;
        else
            dir = Direction.NORTH;

        ClearHightlight();
        entity.Move(dir);
    }

    private void HighlightDecision()
    {
        foreach (Tile tile in entity.currentTile.orthogonalNeighbors)
        {
            tile.Highlight(false);
        }
    }

    private void ClearHightlight()
    {
        foreach (Tile tile in entity.currentTile.orthogonalNeighbors)
        {
            tile.ClearHighlight();
        }
    }
}