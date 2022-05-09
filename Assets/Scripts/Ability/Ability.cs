using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public AbilitySO data;


    public abstract bool UseAbility(Tile tile);
    public abstract void HighlightDecisions(Tile currentTile);
    public abstract bool Tick(float deltaTime);
}
