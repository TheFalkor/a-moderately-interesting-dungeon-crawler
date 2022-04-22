using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity
{

    [Header("Turn variables")]
    private int actionPoints = 2;
    private int movementPoints = 1;
    private bool attackMode = false;
    private const bool tempAllowCorner = false;


    public void Setup()
    {
        base.Initialize();

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);    // no
        CombatUI.instance.UpdateDefense(defense);
    }


    public bool Tick(float deltaTime)
    {
        if (IsBusy())
            return false;

        if (Input.GetKeyUp(KeyCode.Space))
            TakeDamage(new Damage(10, DamageOrigin.ENEMY, null));

        if (attackMode)
        {
            HighlightDecision(HighlightType.ATTACKABLE, tempAllowCorner);

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit)
                {
                    TargetTile(hit.transform.GetComponent<Tile>(), tempAllowCorner);
                }
            }
        }
        else
        {
            HighlightDecision(HighlightType.WALKABLE);

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
        }
        

        return false;
    }

    public void SetAttackMode(bool attackActive /*, Weapon weapon*/)
    {
        attackMode = attackActive;

        ClearHightlight();
    }

    private void MoveToTile(Tile tile)
    {
        if (!tile || !tile.IsWalkable())
            return;

        if (!currentTile.orthogonalNeighbors.Contains(tile))
            return;

        Vector2Int deltaPosition = currentTile.GetPosition() - tile.GetPosition();
        Direction dir;

        if (deltaPosition.x < 0)
            dir = Direction.EAST;
        else if (deltaPosition.x > 0)
            dir = Direction.WEST;
        else if (deltaPosition.y < 0)
            dir = Direction.SOUTH;
        else
            dir = Direction.NORTH;

        if (movementPoints > 0)
            movementPoints--;
        else
            actionPoints--;

        ClearHightlight();
        Move(dir);
    }

    private void TargetTile(Tile tile, bool allowCorners = false)
    {
        if (!tile || !tile.IsOccupied())
            return;

        if (!allowCorners && currentTile.diagonalNeighbors.Contains(tile))
            return;

        if (!currentTile.orthogonalNeighbors.Contains(tile))
            return;

        print("ATTACKED: " + tile.GetPosition());
        tile.AttackTile(new Damage(baseMeleeDamage, DamageOrigin.FRIENDLY));
    }

    private void HighlightDecision(HighlightType type, bool allowCorners = false)
    {
        foreach (Tile tile in currentTile.orthogonalNeighbors)
        {
            tile.Highlight(type);
        }

        if (!allowCorners)
            return;

        foreach (Tile tile in currentTile.diagonalNeighbors)
        {
            tile.Highlight(type);
        }
    }

    private void ClearHightlight()
    {
        foreach (Tile tile in currentTile.orthogonalNeighbors)
        {
            tile.ClearHighlight();
        }

        foreach (Tile tile in currentTile.diagonalNeighbors)
        {
            tile.ClearHighlight();
        }
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
    }

    public override void Heal(int health)
    {
        base.Heal(health);

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
    }

    protected override void Death()
    {
        Debug.Log("player died");
        // End game
    }
}