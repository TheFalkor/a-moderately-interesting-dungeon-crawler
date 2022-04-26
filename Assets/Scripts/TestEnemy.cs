using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Entity
{
    private bool turnEnded = false;
    private float timer = 0;

    public override void PreTurn()
    {
        currentMovementPoints = maxActionPoints;
        currentActionPoints = maxActionPoints;
        timer = 0;
        turnEnded = false;
    }

    public override bool Tick(float deltaTime)
    {
        if (turnEnded)
            return true;

        Debug.Log("barrels turn " + transform.position);

        timer += deltaTime;

        transform.Rotate(new Vector3(0, 0, Time.deltaTime * 450 * transform.localScale.x));

        if (timer > 2.5f)
        {
            turnEnded = true;
            transform.eulerAngles = Vector3.zero;
        }

        return false;
    }
}
