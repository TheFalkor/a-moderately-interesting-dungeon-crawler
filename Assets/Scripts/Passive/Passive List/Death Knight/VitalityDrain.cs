using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitalityDrain : Passive
{
    public override void OnPreTurn(Entity enemy)
    {
        for (int i = 0; i < enemy.activeStatusEffects.Count; i++)
        {
            StatusEffect effect = enemy.activeStatusEffects[i];

            if (effect.type == StatusType.DEATHMARK)
            {
                enemy.AddStatusEffect(new StatusEffect(StatusType.STRENGHT_DRAIN, 1));
            }
        }
    }
}
