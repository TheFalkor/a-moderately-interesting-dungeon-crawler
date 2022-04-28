using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity
{
    [Header("Audio")]
    private AudioKor audioKor;
    public Animator temporaryAnimatorDeath;

    [SerializeField] protected ClassStatsSO classStat;

    [Header("Turn variables")]
    private PlayerState state = PlayerState.MOVE_STATE;
    private bool turnEnded = false;
    private bool allowCorner = true;
    [Space]
    private Ability selectedAbility;
    private bool abilityActive = false;

    [Header("Runtime Variables")]
    private LayerMask tileMask;

    [Header("Inventory")]
    private NonFinalInventoryInterface face;
    public List<ScriptableItem> startingInventory;
    public bool canEquipInCombat = false;
    List<InventoryItem> CombatUsableItems=new List<InventoryItem>();


    public void Setup(BaseStatsSO newBaseStat = null, ClassStatsSO newClassStat = null)
    {
        if (audioKor != null)
            return;

        tileMask = LayerMask.GetMask("Tile");

        if (newBaseStat)
        {
            baseStat = newBaseStat;
            classStat = newClassStat;
        }   


        // Temporary
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = baseStat.entitySprite;
        // !Temporary

        base.Initialize();

        /*currentHealth += classStat.bonusHealth;
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        baseRangeDamage += classStat.bonusRangeDamage;*/

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);    // no
        CombatUI.instance.UpdateDefense(defense);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);

        face = gameObject.GetComponent<NonFinalInventoryInterface>();
        face.UpdateSprites();
        
        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    public void ResestPosition(Vector2 position)
    {
        currentTile.SetOccupant(null);
        transform.position = position;
        currentTile = GridManager.instance.GetTileWorld(transform.position);
        currentTile.SetOccupant(this);

        UpdateLayerIndex();
        targetPosition = transform.position;
    }

    public void SetUpInventory() 
    {
        inventory = new Inventory();
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
        {
            DashAbility da = new DashAbility();

            da.HighlightDecisions(currentTile);
        }

        switch (state)
        {
            case PlayerState.MOVE_STATE:
                if (currentActionPoints > 0 || currentMovementPoints > 0)
                    HighlightDecisions(HighlightType.WALKABLE, allowCorner);

                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, tileMask);

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
                break;

            case PlayerState.ATTACK_STATE:
                // Should not be in update
                /*if (inventory != null)
                {
                    switch (inventory.GetEquipedWeaponType())
                    {
                        case WeaponType.SWORD: allowCorner = true; break;
                        default: allowCorner = false; break;
                    }
                }*/
                // !Should not be in update

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
                break;

            case PlayerState.ABILITY_STATE:
                if (abilityActive)
                {
                    if (selectedAbility.Tick(deltaTime))
                    {
                        SelectAbility(null);
                        abilityActive = false;
                        state = PlayerState.MOVE_STATE;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                    if (hit)
                    {
                        Tile tile = hit.transform.GetComponent<Tile>();
                        
                        if (selectedAbility.UseAbility(tile))
                        {
                            abilityActive = true;
                            currentActionPoints--;  // Dash tmp cost

                            CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
                        }
                    }
                }
                break;
        }


        return false;
    }

    public void SetAttackMode(bool attackActive /*, Weapon weapon*/)
    {
        if (attackActive)
            state = PlayerState.ATTACK_STATE;
        else
            state = PlayerState.MOVE_STATE;

        GridManager.instance.ClearAllHighlights();
    }

    public bool SelectAbility(Ability ability)
    {
        if (currentActionPoints == 0)
            return false;

        selectedAbility = ability;

        if (selectedAbility == null)
        {
            GridManager.instance.ClearAllHighlights();
            state = PlayerState.MOVE_STATE;
            selectedAbility = null;
            return false;
        }

        ClearHightlight();
        state = PlayerState.ABILITY_STATE;
        selectedAbility.HighlightDecisions(currentTile);
        return true;
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
        temporaryAnimatorDeath.SetBool("Closed", true);
        Debug.Log("player died");
        // End game
    }

    public override void UseItem(int index)
    {
        base.UseItem(index);
        UpdateCombatItems();
        UpdateCombatUI();
        UpdateInventoryInterface();
    }
    public override void UseItem(InventoryItem item)
    {
        
        base.UseItem(item);
        UpdateCombatItems();
        UpdateCombatUI();
        UpdateInventoryInterface();


    }
    public override void GiveItem(InventoryItem item)
    {
        base.GiveItem(item);
        UpdateCombatItems();
        UpdateInventoryInterface();
    }

    public void UseCombatItem(int index) 
    {
        if (index >= 0 && index < CombatUsableItems.Count) 
        {
            UseItem(CombatUsableItems[index]);
        }
        
    }
    private void UpdateCombatItems() 
    {
        CombatUsableItems.Clear();
        for(int i=0;i<inventory.GetAmountOfItems(); i++) 
        {
            InventoryItem item = inventory.GetItem(i);
            if (item.GetItemType() != ItemType.EQUIPMENT||canEquipInCombat) 
            {
                CombatUsableItems.Add(item);
            }
        }
    }

    public Sprite GetItemImage(int index)
    {
        if (inventory!=null) 
        {
            InventoryItem item = inventory.GetItem(index);
            if (item != null)
            {
                return item.GetSprite();
            }
        }
        return null;
    }
    public Sprite GetItemImageCombat(int index)
    {

        if (index >= 0 && index < CombatUsableItems.Count)
        {
            InventoryItem item = CombatUsableItems[index];
            if (item != null)
            {
                return item.GetSprite();
            }
        }


        return null;
    }
    public Sprite GetEquipedItemImage(EquipmentType equipment,int slot) 
    {
        if (inventory != null) 
        {
            InventoryItem item=inventory.GetEquipedItem(equipment,slot);
            if (item != null) 
            {
                return item.GetSprite();
            }
        }
        return null;
    }

    public int GetStackSize(int index) 
    {
        if (inventory != null)
        {
            InventoryItem item = inventory.GetItem(index);
            if (item != null)
            {
                return item.GetStackAmount();
            }
        }
        return 0;
    }

    public int GetStackSizeCombat(int index)
    {
        if (index >= 0 && index < CombatUsableItems.Count)
        {
            InventoryItem item = CombatUsableItems[index];
            if (item != null)
            {
                return item.GetStackAmount();
            }
        }
        return 0;
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

		maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
		
        UpdateCombatUI();

        if (CombatUI.instance!=null) 
        {
            CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
            CombatUI.instance.UpdateAttack(baseMeleeDamage);
            CombatUI.instance.UpdateDefense(defense);
        }
        
        UpdateCombatUI();
    }
    private void UpdateCombatUI() 
    {
        if (CombatUI.instance != null)
        {
            CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
            CombatUI.instance.UpdateAttack(baseMeleeDamage);
            CombatUI.instance.UpdateDefense(defense);

            CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        }
    }
    private void UpdateInventoryInterface() 
    {
        if (face != null)
        {
            face.UpdateSprites();
        }
    }

    
}