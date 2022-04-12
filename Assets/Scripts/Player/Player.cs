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
        HighlightDecision();
        if (!entity.IsBusy())
        {
            if (Input.GetKeyUp(KeyCode.W))
                entity.Move(Direction.NORTH);
            if (Input.GetKeyUp(KeyCode.A))
                entity.Move(Direction.WEST);
            if (Input.GetKeyUp(KeyCode.S))
                entity.Move(Direction.SOUTH);
            if (Input.GetKeyUp(KeyCode.D))
                entity.Move(Direction.EAST);
        }

        return false;
    }

    private void HighlightDecision()
    {
        foreach (Tile t in entity.currentTile.orthogonalNeighbors)
        {
            t.Highlight();
        }
    }

    private void ClearHightlight()
    {
        
    }
}
