using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornDash : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnAbilityUsed += OnAbilityUsed;
    }

    private void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        if (ability != AbilityID.FLUTTER_DASH)
            return;

        foreach (Entity entity in affectedEnemies)
        {
            entity.TakeCleanDamage(data.passiveValue, DamageOrigin.FRIENDLY);
        }
    }
}
