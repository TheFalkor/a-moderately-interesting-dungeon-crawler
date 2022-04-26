using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : Ability
{
    [Header("Runtime Variables")]
    private Tile targetTile;


    public override bool UseAbility(Tile tile)
    {
        // if tile is not attackble for any reason
        // return false

        targetTile = tile;

        // Do attackie
        // return true

        return false;
    }

    public override void HighlightDecisions()
    {
        throw new System.NotImplementedException();
    }

    public override bool Tick(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}
