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
    [Space]
    private Item selectedItem;

    [Header("Runtime Variables")]
    private LayerMask tileMask;
    private Inventory inventory;


    public void Setup(BaseStatsSO newBaseStat = null, ClassStatsSO newClassStat = null)
    {
        if (audioKor != null)
            return;

        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
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

        currentHealth += classStat.bonusHealth;
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        baseRangeDamage += classStat.bonusRangeDamage;

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
        CombatUI.instance.UpdateAttack(baseMeleeDamage);    // no
        CombatUI.instance.UpdateDefense(defense);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        
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

    public override void PreTurn()
    {
        base.PreTurn();

        SelectAbility(null);
        HotbarUI.instance.SelectItem(-1);

        state = PlayerState.MOVE_STATE;
        turnEnded = false;

        CombatUI.instance.SetAttackButton(false);
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
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, tileMask);

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
                if (currentActionPoints > 0)
                    HighlightDecisions(HighlightType.ATTACKABLE, allowCorner);


                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, tileMask);

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

                        CombatUI.instance.SelectAbility(-1);
                        GridManager.instance.ClearAllHighlights();
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, tileMask);

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

            case PlayerState.ITEM_STATE:
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, tileMask);
                    if (hit)
                    {
                    Debug.Log(hit.transform.name);
                        Tile tile = hit.transform.GetComponent<Tile>();

                        if (selectedItem.UseItem(tile))
                        {
                            SelectItem(-1);
                            state = PlayerState.MOVE_STATE;

                            HotbarUI.instance.SelectItem(-1);
                            GridManager.instance.ClearAllHighlights();
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
            return false;
        }

        ClearHightlight();
        state = PlayerState.ABILITY_STATE;
        selectedAbility.HighlightDecisions(currentTile);
        return true;
    }

    public bool SelectItem(int inventoryIndex)
    {
        if (inventoryIndex == -1)
        {
            GridManager.instance.ClearAllHighlights();
            state = PlayerState.MOVE_STATE;
            return false;
        }

        selectedItem = inventory.items[inventoryIndex];

        ClearHightlight();
        state = PlayerState.ITEM_STATE;
        selectedItem.HighlightDecision(currentTile);
        return true;
    }

    public void EndTurn()
    {
        GridManager.instance.ClearAllHighlights();
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


        tile.AttackTile(new Damage(baseMeleeDamage, DamageOrigin.FRIENDLY));
        currentActionPoints--;

        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        
        audioKor.PlaySFX("SLASH");
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

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth);
    }

    protected override void Death()
    {
        temporaryAnimatorDeath.SetBool("Closed", true);
        Debug.Log("player died");
        // End game
    }    
}