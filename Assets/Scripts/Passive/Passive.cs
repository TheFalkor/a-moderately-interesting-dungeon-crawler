using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive
{

    public virtual void OnPreTurn(Entity entity)
    {

    }

    public virtual void OnEndTurn(Entity entity)
    {

    }

    public virtual void OnPlayerTakeDamage(Entity entity)
    {

    }

    public virtual void OnEnemyTakeDamage(Entity entity)
    {

    }

    public virtual void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {

    }
}
