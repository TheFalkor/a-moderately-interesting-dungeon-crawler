using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Occupant
{
    [SerializeField] protected ClassStatsSO classStat;

    [Header("Entity Settings")]
    private const float ANIMATED_MOVEMENT_SPEED = 3.5f;

    [Header("Runtime Variables")]
    private Vector2 targetPosition;
    protected bool isMoving = false;


    public override void Initialize()
    {
        base.Initialize();

        currentHealth += classStat.bonusHealth;
        maxhealth += classStat.bonusHealth;
        defense += classStat.bonusDefense;
        baseMeleeDamage += classStat.bonusMeleeDamage;
        baseRangeDamage += classStat.bonusRangeDamage;
        
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * ANIMATED_MOVEMENT_SPEED);
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * 90));

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
                break;

            case Direction.EAST:
                transform.localScale = new Vector3(1f, 1f);
                targetPosition += new Vector2(1, 0);
                break;

            case Direction.SOUTH:
                targetPosition += new Vector2(0, -1);
                break;

            case Direction.WEST:
                transform.localScale = new Vector3(-1, 1);
                targetPosition += new Vector2(-1, 0);
                break;
        }

        currentTile.SetOccupant(null);
        currentTile = GridManager.instance.GetTileWorld(targetPosition);
        currentTile.SetOccupant(this);
    }
}
