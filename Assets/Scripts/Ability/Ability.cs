using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public AbilitySO data;
    public readonly List<Entity> affectedEnemies = new List<Entity>();
    public int cooldown = 0;


    public abstract bool UseAbility(Tile tile);
    public abstract void HighlightDecisions(Tile currentTile);
    public abstract bool Tick(float deltaTime);
}
