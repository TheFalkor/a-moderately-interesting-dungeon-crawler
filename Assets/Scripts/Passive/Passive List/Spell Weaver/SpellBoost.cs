using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBoost : Passive
{
    public override void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        DungeonManager.instance.player.ChangeMP(data.passiveValue);
    }
}
