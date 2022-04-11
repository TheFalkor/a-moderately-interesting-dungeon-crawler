using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Occupant
{
    
    void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            Move(Direction.NORTH);
        if (Input.GetKeyUp(KeyCode.A))
            Move(Direction.WEST);
        if (Input.GetKeyUp(KeyCode.S))
            Move(Direction.SOUTH);
        if (Input.GetKeyUp(KeyCode.D))
            Move(Direction.EAST);
    }

    public void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.NORTH:
                transform.position += new Vector3(0, 1);
                break;

            case Direction.EAST:
                transform.position += new Vector3(1, 0);
                break;

            case Direction.SOUTH:
                transform.position += new Vector3(0, -1);
                break;

            case Direction.WEST:
                transform.position += new Vector3(-1, 0);
                break;
        }
    }
}
