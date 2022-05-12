using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive
{
    public PassiveSO data;

    public virtual void OnPreTurn(Entity entity)
    {

    }

    public virtual void OnEndTurn(Entity entity)
    {

    }

    public virtual void OnPlayerTakeDamage(Entity enemy)
    {

    }

    public virtual void OnEnemyTakeDamage(Entity enemy)
    {

    }

    public virtual void OnEnemyDeath(Entity enemy)
    {

    }

    public virtual void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {

    }

    public virtual void OnPlayerMove(Player player)
    {

    }

    public virtual void OnPlayerAttack(Player player)
    {

    }
}
