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

        if (affectedEnemies.Count > 0)
        {
            Transform temp = DungeonManager.instance.player.transform.Find("SiphonDeathPlayerShieldVFX(Clone)");
            if (!temp)
            {
                GameObject tempSelf = Object.Instantiate(data.passiveVFX[0], DungeonManager.instance.player.transform.position, Quaternion.identity);
                tempSelf.transform.parent = DungeonManager.instance.player.transform;
            }
        }
    }
}
