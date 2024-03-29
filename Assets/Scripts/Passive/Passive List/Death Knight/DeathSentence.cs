using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSentence : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Entity enemy)
    {
        foreach (StatusEffect effect in enemy.activeStatusEffects)
        {
            if (effect.type == StatusType.DEATHMARK)
            {
                DungeonManager.instance.player.Heal(data.passiveValue);
            }
        }
    }
}
