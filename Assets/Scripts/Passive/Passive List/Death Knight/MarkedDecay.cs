using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedDecay : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnEndTurn += OnEndTurn;
    }

    private void OnEndTurn(Entity entity)
    {
        foreach (StatusEffect effect in entity.activeStatusEffects)
        {
            if (effect.type == StatusType.DEATHMARK)
            {
                entity.TakeCleanDamage(data.passiveValue, DamageOrigin.NEUTRAL);
            }
        }
    }
}
