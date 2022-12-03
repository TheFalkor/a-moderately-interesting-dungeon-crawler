using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBoost : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnAbilityUsed += OnAbilityUsed;
    }

    private void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        DungeonManager.instance.player.ChangeCurrentMP(data.passiveValue);
    }
}
