using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRift : Passive
{
    public override void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        if (!(ability == AbilityID.TIME_PULSE))
            return;

        foreach (Entity e in affectedEnemies)
            e.AddStatusEffect(new StatusEffect(StatusType.ROOTED, 2));
    }
}
