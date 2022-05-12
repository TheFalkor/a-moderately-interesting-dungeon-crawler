using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBubble : Passive
{
    public override void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        if (!(ability == AbilityID.TIME_PULSE))
            return;

        DungeonManager.instance.player.AddShield(data.passiveValue * affectedEnemies.Count);
    }
}
