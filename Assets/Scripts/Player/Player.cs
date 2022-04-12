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
        if (!entity.IsBusy())
        {
            HighlightDecision();
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit)
                {
                    Tile tile = hit.transform.GetComponent<Tile>();

                    if (tile && tile.IsWalkable())
                    {
                        MoveToTile(tile);
                    }
                }
            }


            if (Input.GetKeyUp(KeyCode.W))
            {
                ClearHightlight();
                entity.Move(Direction.NORTH);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                ClearHightlight();
                entity.Move(Direction.WEST);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                ClearHightlight();
                entity.Move(Direction.SOUTH);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                ClearHightlight();
                entity.Move(Direction.EAST);
            }
        }

        return false;
    }

    private void MoveToTile(Tile tile)
    {
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
