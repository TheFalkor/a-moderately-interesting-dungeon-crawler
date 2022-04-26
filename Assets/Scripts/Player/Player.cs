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
    private bool allowCorner = false;
    
    [Header("Inventory")]
    
    private NonFinalInventoryInterface face;//temporary and needs to be reworked
    public List<ScriptableItem> startingInventory;


    public void Setup(BaseStatsSO baseStat = null, ClassStatsSO classStat = null)
    {
        if (baseStat)
        {
            this.baseStat = baseStat;
            this.classStat = classStat;
        }


        // Temporary
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = this.baseStat.entitySprite;
        // !Temporary

        base.Initialize();

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);    // no
        CombatUI.instance.UpdateDefense(defense);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);

        face = gameObject.GetComponent<NonFinalInventoryInterface>();
        face.UpdateSprites();

        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    public void SetUpInventory() 
    {
        inventory = new Inventory();//only player is currently using inventory. can be moved to a parent class
        inventory.SetOwner(this);
        inventory.CreateEquipmentInventory();
        GiveStartingItems();
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
            if (inventory != null) 
            {
                switch (inventory.GetEquipedWeaponType()) 
                {
                    case WeaponType.SWORD:allowCorner = true; break;
                    default: allowCorner = false;break;
                }
            }
            if (currentActionPoints > 0)
                HighlightDecisions(HighlightType.ATTACKABLE, allowCorner);
            

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit)
                {
                    TargetTile(hit.transform.GetComponent<Tile>(), allowCorner);
                }
            }
        }
        else
        {
            if (currentActionPoints > 0 || currentMovementPoints > 0)
                HighlightDecisions(HighlightType.WALKABLE, allowCorner);

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
            TargetTile(tile, allowCorner);
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

        if (!allowCorners && currentTile.diagonalNeighbors.Contains(tile))
            return;

        if (!currentTile.orthogonalNeighbors.Contains(tile) && !currentTile.diagonalNeighbors.Contains(tile))
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

    private void HighlightDecisions(HighlightType type, bool allowCorners = false)
    {
        foreach (Tile tile in currentTile.orthogonalNeighbors)
        {
            tile.Highlight(type);
        }

        if (!allowCorners)
            return;

        foreach (Tile tile in currentTile.diagonalNeighbors)
        {
            if (type == HighlightType.WALKABLE && !tile.IsOccupied())
                continue;

            tile.Highlight(HighlightType.ATTACKABLE);
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
        if (CombatUI.instance != null) 
        {
            CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        }
        
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

        if (CombatUI.instance!=null) 
        {

            CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
            CombatUI.instance.UpdateAttack(baseMeleeDamage);
            CombatUI.instance.UpdateDefense(defense);
        }
        
    }
}