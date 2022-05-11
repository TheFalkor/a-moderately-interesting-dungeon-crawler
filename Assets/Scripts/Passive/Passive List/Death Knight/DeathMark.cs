using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMark : Passive
{
    public override void OnEnemyTakeDamage(Entity enemy)
    {
        foreach (StatusEffect effect in enemy.activeStatusEffects)
        {
            if (effect.type == StatusType.DEATHMARK)
            {
                enemy.TakeCleanDamage(data.passiveValue, DamageOrigin.FRIENDLY);
            }
        }

        enemy.AddStatusEffect(new StatusEffect(StatusType.DEATHMARK, 2));
    }

}
