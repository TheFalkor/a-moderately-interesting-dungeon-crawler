using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Occupant
{

    [Header("Entity Stats")]
    protected int maxMovementPoints;
    protected int currentMovementPoints;
    public int maxActionPoints;     // mr.NOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
    protected int currentActionPoints;

    [Header("Entity Settings")]
    private const float ANIMATED_MOVEMENT_SPEED = 3.5f;

    [Header("Runtime Variables")]
    protected Vector2 targetPosition;
    protected bool isMoving = false;
    protected readonly List<Tile> tilesInRange = new List<Tile>();

    public override void Initialize()
    {
        base.Initialize();
        currentHealth = maxhealth;

        maxMovementPoints = baseStat.movementPoints;
        currentMovementPoints = maxMovementPoints;
        maxActionPoints = baseStat.actionPoints;
        currentActionPoints = maxActionPoints;

        targetPosition = transform.position;       
    }

    public abstract bool Tick(float deltaTime);
    public virtual void PreTurn()
    {
        currentMovementPoints = maxMovementPoints;
        currentActionPoints = maxActionPoints;

        PassiveManager.instance.OnPreTurn(this);

        UpdateStatusEffects();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * ANIMATED_MOVEMENT_SPEED);
            if (targetPosition.y == transform.position.y)
                transform.Rotate(new Vector3(0, 0, Time.deltaTime * 90 * transform.localScale.x));

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

        targetPosition = currentTile.transform.position;
        isMoving = true;
        switch (direction)
        {
            case Direction.NORTH:
                targetPosition += new Vector2(0, 1);
                render.sortingOrder -= 5;
                break;

            case Direction.EAST:
                transform.localScale = new Vector3(-1f, 1f);
                targetPosition += new Vector2(1, 0);
                break;

            case Direction.SOUTH:
                targetPosition += new Vector2(0, -1);
                render.sortingOrder += 5;
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
}
