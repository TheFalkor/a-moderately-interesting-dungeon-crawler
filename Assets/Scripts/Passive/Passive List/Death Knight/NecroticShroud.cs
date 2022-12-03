using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticShroud : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnPlayerTakeDamage += OnPlayerTakeDamage;
    }

    private void OnPlayerTakeDamage(Entity enemy)
    {
        enemy.AddStatusEffect(new StatusEffect(StatusType.DEATHMARK, 2));
    }
}
