using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity
{
    [Header("Audio")]
    public Animator temporaryAnimatorDeath;

    public ClassStatsSO classStat;

    [Header("Turn variables")]
    private PlayerState state = PlayerState.MOVE_STATE;
    private bool turnEnded = false;
    [Space]
    private Ability selectedAbility;
    [HideInInspector] public bool abilityActive = false;
    [Space]
    private Item selectedItem;
    private int selectedItemIndex = -1;

    [Header("Runtime Variables")]
    private LayerMask tileMask;
    private Inventory inventory;
    private bool ranOutOfPoints = false;

	[Header("VFX Prefabs")]
    public GameObject HealVFX;

    // EVENT DELEGATEs
    public delegate void MoveEvent(Tile t);
    public static event MoveEvent playerMove;

    public delegate void AttackEvent();
    public static event AttackEvent playerAttack;

    public delegate void EndOfTurnEvent();
    public static event EndOfTurnEvent playerEndTurn;


    public void Setup(BaseStatsSO newBaseStat = null, ClassStatsSO newClassStat = null)
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        tileMask = LayerMask.GetMask("Tile");

        if (newBaseStat)
        {
            baseStat = newBaseStat;
            classStat = newClassStat;
        }   

        

        // Temporary
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = baseStat.entitySprite;
        // !Temporary

        base.Initialize();

        currentHealth += classStat.bonusHealth;
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        baseRangeDamage += classStat.bonusRangeDamage;
    }

    public void RecalculateStats()
    {
        int newHealth = baseStat.maxHealth + classStat.bonusHealth;
        if (inventory.equippedArmor != null)
            newHealth += inventory.equippedArmor.health;

        int newDamage = baseStat.baseMeleeDamage + classStat.bonusMeleeDamage + inventory.equippedWeapon.weaponDamage;
        if (inventory.equippedArmor != null)
            newDamage += inventory.equippedArmor.damage;

        int newDefense = baseStat.defense + classStat.bonusDefense + inventory.equippedWeapon.weaponData.bonusDefense;
        if (inventory.equippedArmor != null)
            newDefense += inventory.equippedArmor.defense;

        if (newHealth != maxhealth)
        {
            float perc = currentHealth / (float)maxhealth;
            maxhealth = newHealth;
            currentHealth = Mathf.RoundToInt(perc * maxhealth);
        }

        maxMovementPoints = baseStat.movementPoints + inventory.equippedWeapon.weaponData.bonusMP;

        meleeDamage = newDamage;
        defense = newDefense;
    }

    public void ResetPlayer()
    {
        transform.position = new Vector2(4.5f, 0.5f);
        currentTile = GridManager.instance.GetTileWorld(transform.position);
        currentTile.SetOccupant(this);

        Transform temp = transform.Find("SiphonDeathPlayerShieldVFX(Clone)");
        if (temp)
            Destroy(temp.gameObject);

        shield = 0;

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth, shield);
        CombatUI.instance.UpdateAttack(meleeDamage);
        CombatUI.instance.UpdateDefense(defense);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);

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

        RecalculateStats();

        HotbarUI.instance.EnableHotbar();

        CombatUI.instance.SetAttackButton(false);
        CombatUI.instance.UpdateAttack(meleeDamage);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
    }

    public override bool Tick(float deltaTime)
    {
        if (turnEnded)
            return true;

        if (IsBusy())
            return false;

        if (currentActionPoints == 0 && !ranOutOfPoints)
        {
            ranOutOfPoints = true;
            CombatUI.instance.EnableEndTurnGlow();
        }

        switch (state)
        {
            case PlayerState.MOVE_STATE:
                inventory.equippedWeapon.CheckHighlights();

                if (currentActionPoints > 0 || currentMovementPoints > 0)
                    HighlightDecisions();


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
                {
                    inventory.equippedWeapon.HighlightDecision(currentTile);
                }


                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, tileMask);

                    if (hit)
                    {
                        StrikeTiles(inventory.equippedWeapon.Attack(hit.transform.GetComponent<Tile>()));
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

                        ServiceLocator.Get<EventManager>().OnAbilityUsed?.Invoke(selectedAbility.data.abilityType, selectedAbility.affectedEnemies);

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
                            if (selectedAbility.data.abilitySFX != "")
                                audioCore.PlaySFX(selectedAbility.data.abilitySFX);
                            abilityActive = true;
                            currentActionPoints -= selectedAbility.data.actionPointCost;
                            selectedAbility.cooldown = selectedAbility.data.cooldown;

                            CombatUI.instance.UpdateAbilityUI();
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
                        Tile tile = hit.transform.GetComponent<Tile>();

                        if (selectedItem.UseItem(tile))
                        {
                            SelectItem(-1);
                            state = PlayerState.MOVE_STATE;

                            HotbarUI.instance.SelectItem(-1);
                            CombatUI.instance.SetAttackButton(false);

                            inventory.RemoveItem(selectedItemIndex);
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
        if (abilityActive)
            return false;

        selectedAbility = ability;

        if (selectedAbility == null)
        {
            GridManager.instance.ClearAllHighlights();
            state = PlayerState.MOVE_STATE;
            return false;
        }

        if (currentActionPoints < ability.data.actionPointCost)
            return false;

        GridManager.instance.ClearAllHighlights();
        state = PlayerState.ABILITY_STATE;
        selectedAbility.HighlightDecisions(currentTile);
        audioCore.PlaySFX("SELECT");
        return true;
    }

    public bool SelectItem(int inventoryIndex)
    {
        if (inventoryIndex == -1)
        {
            if (state == PlayerState.ITEM_STATE)
            {
                GridManager.instance.ClearAllHighlights();
                state = PlayerState.MOVE_STATE;
            }
            return false;
        }

        audioCore.PlaySFX("SELECT");

        selectedItem = inventory.items[inventoryIndex];
        selectedItemIndex = inventoryIndex;

        GridManager.instance.ClearAllHighlights();
        state = PlayerState.ITEM_STATE;
        selectedItem.HighlightDecision(currentTile);
        return true;
    }

    public bool EndTurn()
    {
        if (abilityActive || IsBusy())
            return false;

        ranOutOfPoints = false;
        GridManager.instance.ClearAllHighlights();
        CombatUI.instance.DisableAbilityUI();
        HotbarUI.instance.DisableItemUI();
        turnEnded = true;

        if (playerEndTurn != null)
            playerEndTurn();

        return true;
    }

    public void ChangeCurrentAP(int difference)
    {
        currentActionPoints += difference;

        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
    }

    public void ChangeMaxAP(int difference)
    {
        maxActionPoints += difference;
    }

    public void ChangeCurrentMP(int difference)
    {
        currentMovementPoints += difference;
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
    }

    public void ChangeMeleeDamage(int difference)
    {
        meleeDamage += difference;
        CombatUI.instance.UpdateAttack(meleeDamage);
    }

    private void MoveToTile(Tile tile)
    {
        if (currentActionPoints == 0 && currentMovementPoints == 0)
            return;

        if (!tile || !tile.IsWalkable())
            return;

        if (tile.IsOccupied())
        {
            if (currentActionPoints > 0)
                StrikeTiles(inventory.equippedWeapon.Attack(tile));
            return;
        }

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

        if (currentMovementPoints > 0)
            currentMovementPoints--;
        else
            currentActionPoints--;

        ServiceLocator.Get<EventManager>().OnPlayerMove?.Invoke(this);
        playerMove?.Invoke(tile);
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);

        GridManager.instance.ClearAllHighlights();
        Move(dir);

        audioCore.PlaySFX("MOVE");
    }

    private void StrikeTiles(List<WeaponStrike> strikes)
    {
        if (currentActionPoints == 0)
            return;

        if (strikes.Count == 0)
            return;

        Vector2 faceDirection = new Vector2(currentTile.GetPosition().x - strikes[0].tile.GetPosition().x, 0);
        if (faceDirection.x != 0)
            transform.localScale = new Vector3(faceDirection.normalized.x, 1);

        foreach (WeaponStrike ws in strikes)
        {
            int attackDamage = Mathf.RoundToInt(meleeDamage * ws.splashMultiplier);
            Attack(ws.tile, new Damage(attackDamage, DamageOrigin.FRIENDLY, ws.weaponDamage.statusEffects));
        }

        transform.GetChild(0).GetChild(0).GetComponent<Animator>().Play("Attack");

        currentActionPoints--;
        ServiceLocator.Get<EventManager>().OnPlayerAttack?.Invoke(this);

        playerAttack?.Invoke();
        CombatUI.instance.UpdateActionPoints(currentMovementPoints, currentActionPoints);
        GridManager.instance.ClearAllHighlights();

        switch (inventory.equippedWeapon.weaponType)
        {
            case WeaponType.SWORD:
                audioCore.PlaySFX("SLASH");
                break;
            case WeaponType.SPEAR:
                audioCore.PlaySFX("SLASH"); // PUT CORRECT SFX
                break;
            case WeaponType.HAMMER:
                audioCore.PlaySFX("SLASH"); // PUT CORRECT SFX
                break;
        }
    }

    private void HighlightDecisions()
    {
        foreach (Tile tile in currentTile.orthogonalNeighbors)
            if (!tile.IsOccupied())
                tile.Highlight(HighlightType.WALKABLE);

        if (currentActionPoints > 0)
            inventory.equippedWeapon.ExtraHighlight(currentTile);
    }

    public override void TakeDamage(Damage damage, Occupant attacker)
    {
        base.TakeDamage(damage, attacker);

        if (attacker is Entity entity)
            ServiceLocator.Get<EventManager>().OnPlayerTakeDamage?.Invoke(entity);

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth, shield);
    }

    public override void Heal(int health)
    {
        base.Heal(health);

        if (CombatUI.instance)
            CombatUI.instance.UpdateHealth(currentHealth, maxhealth, shield);

        overhead.UpdateHealthbar(currentHealth / (float)maxhealth);

        GameObject healVFX = Instantiate(HealVFX, gameObject.transform.GetChild(0));
        Destroy(healVFX, 0.5f);
        audioCore.PlaySFX("HEALTH_GAIN");
    }

    public void AddShield(int shield)
    {
        this.shield += shield;

        CombatUI.instance.UpdateHealth(currentHealth, maxhealth, this.shield);
        audioCore.PlaySFX("HEALTH_GAIN");
    }

    public void UpdateInventoryStats()
    {
        RecalculateStats();
        InventoryUI.instance.UpdatePlayerStats(currentHealth, maxhealth, defense, meleeDamage);
    }

    public void UpdateTownStats()
    {
        RecalculateStats();
        TownManager.instance.UpdatePlayerStats(currentHealth, maxhealth, defense, meleeDamage);
    }

    public int GetWeaponDamage()
    {
        return inventory.equippedWeapon.weaponDamage;
    }

    public int GetCurrentAP()
    {
        return currentActionPoints;
    }

    protected override void Death()
    {
        transform.GetChild(0).GetChild(0).GetComponent<Animator>().Play("Death");
        Destroy(gameObject, 4 / 15f);
        audioCore.PlaySFX("DEATH");

        EndManager.instance.EndGame(false);
    }
}