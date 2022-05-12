using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConduitOfPower : Passive
{
    public override void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        foreach (Enemy e in affectedEnemies)
            e.TakeCleanDamage(3, DamageOrigin.FRIENDLY);
    }
}
