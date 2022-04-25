using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Occupant
{
    [SerializeField] protected ClassStatsSO classStat;

    [Header("Entity Stats")]
    protected int maxMovementPoints;
    protected int currentMovementPoints;
    protected int maxActionPoints;
    protected int currentActionPoints;

    [Header("Entity Settings")]
    private const float ANIMATED_MOVEMENT_SPEED = 3.5f;

    [Header("Runtime Variables")]
    private Vector2 targetPosition;
    protected bool isMoving = false;
    protected readonly List<Tile> tilesInRange = new List<Tile>();


    public override void Initialize()
    {
        base.Initialize();

        currentHealth += classStat.bonusHealth;
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        baseRangeDamage += classStat.bonusRangeDamage;

        maxMovementPoints = baseStat.movementPoints;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = baseStat.actionPoints;
        currentActionPoints = maxActionPoints;

        targetPosition = transform.position;
    }

    public abstract bool Tick(float deltaTime);
    public virtual void PreTurn()
    {
        UpdateStatusEffects();

        currentMovementPoints = maxMovementPoints;
        currentActionPoints = maxActionPoints;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * ANIMATED_MOVEMENT_SPEED);
            if (targetPosition.y == transform.position.y)
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * 90 * transform.localScale.x));
            // else
            //    transform.Rotate(new Vector3(0, 0, Time.deltaTime * 90 * transform.localScale.x));

            if (targetPosition == (Vector2)transform.position)
            {
                isMoving = false;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    protected bool IsBusy()
    {
        return isMoving;
    }

    protected void Move(Direction direction)
    {
        if (isMoving)
            return;

        isMoving = true;
        switch (direction)
        {
            case Direction.NORTH:
                targetPosition += new Vector2(0, 1);
                render.sortingOrder--;
                break;

            case Direction.EAST:
                transform.localScale = new Vector3(-1f, 1f);
                targetPosition += new Vector2(1, 0);
                break;

            case Direction.SOUTH:
                targetPosition += new Vector2(0, -1);
                render.sortingOrder++;
                break;

            case Direction.WEST:
                transform.localScale = new Vector3(1, 1);
                targetPosition += new Vector2(-1, 0);
                break;
        }
        
        currentTile.SetOccupant(null);
        currentTile = GridManager.instance.GetTileWorld(targetPosition);
        currentTile.SetOccupant(this);
    }

    protected override void Death()
    {
        CombatManager.instance.RemoveEntity(this);

        base.Death();
    }

    protected override void UpdateStats()
    {
        base.UpdateStats();
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        maxActionPoints = baseStat.actionPoints;
        if (inventory != null ) 
        {
            if (inventory.GetEquipedWeaponType()==WeaponType.SWORD) 
            {
                maxActionPoints++;
            }
        }

    }

    protected void AttackWithWeapon(Tile tile)
    {
        if (inventory != null && inventory.HasEquipmentInventory())
        {
            switch (inventory.GetEquipedWeaponType())
            {
                case WeaponType.SWORD:AttackWithSword(tile); break;
                default: AttackWithNone(tile); break;
            }
        }
        else
        {
            AttackWithNone(tile);
        }
    }
    protected void AttackWithNone(Tile tile) 
    {
        if (currentActionPoints > 0&& currentTile.orthogonalNeighbors.Contains(tile)) 
        {
            currentActionPoints--;
            Attack(tile, new Damage(baseMeleeDamage, originType));
        }
    }

    protected void AttackWithSword(Tile tile)
    {
        Debug.Log(1);
        if (currentActionPoints>0)
        {
            Debug.Log(2);
            if (currentTile.orthogonalNeighbors.Contains(tile) ||currentTile.diagonalNeighbors.Contains(tile) ) 
            {
                Debug.Log(3);
                Attack(tile, new Damage(baseMeleeDamage, originType));
                currentActionPoints--;
            }
        }
    }
}
