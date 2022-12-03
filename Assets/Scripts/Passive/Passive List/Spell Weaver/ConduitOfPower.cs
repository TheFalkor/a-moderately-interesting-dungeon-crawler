using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConduitOfPower : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnAbilityUsed += OnAbilityUsed;
    }
    private void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        foreach (Enemy e in affectedEnemies)
            e.TakeCleanDamage(data.passiveValue, DamageOrigin.FRIENDLY);
    }
}
