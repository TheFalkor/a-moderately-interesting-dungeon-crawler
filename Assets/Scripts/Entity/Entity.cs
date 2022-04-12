using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Occupant
{
    [SerializeField] protected ClassStatsSO classStat;
    private Vector2 targetPosition;
    protected bool isMoving = false;
    private const float ANIMATED_MOVEMENT_SPEED = 3.5f;


    new void Start()
    {
        // Load stats 1 time
        targetPosition = transform.position;
        currentTile = GridManager.instance.GetTileWorld(targetPosition);
        base.Start();
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

    public bool IsBusy()
    {
        return isMoving;
    }

    public void Move(Direction direction)
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
                transform.localScale = new Vector3(-.7f, .7f);
                targetPosition += new Vector2(1, 0);
                break;

            case Direction.SOUTH:
                targetPosition += new Vector2(0, -1);
                break;

            case Direction.WEST:
                transform.localScale = new Vector3(.7f, .7f);
                targetPosition += new Vector2(-1, 0);
                break;
        }

        currentTile.SetOccupant(null);
        currentTile = GridManager.instance.GetTileWorld(targetPosition);
        currentTile.SetOccupant(this);
    }
}
