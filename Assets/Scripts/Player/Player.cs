using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity
{
    [Header("Audio")]
    private AudioKor audioKor;

    [Header("Turn variables")]
    private bool turnEnded = false;
    private bool attackMode = false;
    private const bool tempAllowCorner = false;
    
    [Header("Inventory")]
    
    private NonFinalInventoryInterface face;//temporary and needs to be reworked
    public List<ScriptableItem> startingInventory;


    public void Setup()
    {
        base.Initialize();

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);    // no
        CombatUI.instance.UpdateDefense(defense);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        inventory = new Inventory();//only player is currently using inventory. can be moved to a parent class

        inventory.SetOwner(this);
        inventory.CreateEquipmentInventory();
        GiveStartingItems();
        
        face = gameObject.GetComponent<NonFinalInventoryInterface>();
        face.UpdateSprites();

        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    public override void PreTurn()
    {
        base.PreTurn();

        turnEnded = false;

        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
    }

    public override bool Tick(float deltaTime)
    {
        if (turnEnded)
            return true;

        if (IsBusy())
            return false;

        if (Input.GetKeyUp(KeyCode.Space))
            TakeDamage(new Damage(10, DamageOrigin.ENEMY, null));

        if (attackMode)
        {
            bool cornerAttack=false;
            if (inventory != null) 
            {
                switch (inventory.GetEquipedWeaponType()) 
                {
                    case WeaponType.SWORD:cornerAttack = true; break;
                    default: cornerAttack = false;break;
                }
            }
            if (currentActionPoints > 0)
                HighlightDecision(HighlightType.ATTACKABLE, cornerAttack);
            

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit)
                {
                    TargetTile(hit.transform.GetComponent<Tile>(), cornerAttack);
                }
            }
        }
        else
        {
            if (currentActionPoints > 0 || currentMovementPoints > 0)
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

    public void EndTurn()
    {
        ClearHightlight();
        turnEnded = true;
    }

    private void MoveToTile(Tile tile)
    {
        if (currentActionPoints == 0 && currentMovementPoints == 0)
            return;

        if (!tile || !tile.IsWalkable())
            return;

        if (tile.IsOccupied())
        {
            TargetTile(tile, true);
            return;
        }

        if (!currentTile.orthogonalNeighbors.Contains(tile))
            return;

        Vector2Int deltaPosition = currentTile.GetPosition() - tile.GetPosition();
        Direction dir;

        audioKor.PlaySFX("MOVE");

        if (deltaPosition.x < 0)
            dir = Direction.EAST;
        else if (deltaPosition.x > 0)
            dir = Direction.WEST;
        else if (deltaPosition.y < 0)
            dir = Direction.SOUTH;
        else
            dir = Direction.NORTH;

        if (currentMovementPoints > 0)
            currentMovementPoints--;
        else
            currentActionPoints--;

        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);

        ClearHightlight();
        Move(dir);
    }

    private void TargetTile(Tile tile, bool allowCorners = false)
    {
        if (currentActionPoints == 0)
            return;

        if (!tile || !tile.IsOccupied())
            return;

        //if (!allowCorners && currentTile.diagonalNeighbors.Contains(tile)) //had to move down to next if statment, otherwise corners do not work.
        //    return;

        if (!(currentTile.orthogonalNeighbors.Contains(tile)|| (allowCorners&& currentTile.diagonalNeighbors.Contains(tile))))
            return;

        ClearHightlight();

        //currentActionPoints--;//moved to attack with weapon
        AttackWithWeapon(tile);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        
        //tile.AttackTile(new Damage(baseMeleeDamage, DamageOrigin.FRIENDLY));//also moved to attack with weapon

        audioKor.PlaySFX("SLASH");

        /*print("ATTACKED: " + tile.GetPosition());
        actionPoints--;
        tile.AttackTile(new Damage(baseMeleeDamage, DamageOrigin.FRIENDLY));*/
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

    public override void UseItem(int index)
    {
        
        base.UseItem(index);
        UpdateStats();
        if (face)
        {
            face.UpdateSprites();
        }
    }
    public Sprite GetItemImage(int index)
    {
        if (inventory != null) 
        {
            InventoryItem item = inventory.GetItem(index);
            if (item != null)
            {
                return item.GetSprite();
            }
        }
        return null;

    }
   
    public void GiveStartingItems()
    {
        foreach (ScriptableItem item in startingInventory)
        {
            GiveItem(item.CreateItem());
        }

    }
    protected override void UpdateStats()
    {
        base.UpdateStats();
        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);
        CombatUI.instance.UpdateDefense(defense);
    }
}