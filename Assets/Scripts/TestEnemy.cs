using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Entity
{
    private bool turnEnded = false;
    private float timer = 0;

    public override void ResetTurn()
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

        if (timer > 5)
            turnEnded = true;

        return false;
    }

    void Start()
    {
        base.Initialize();
    }

    void Update()
    {
        
    }
}
