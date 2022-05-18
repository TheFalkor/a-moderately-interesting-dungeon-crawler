using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticShroud : Passive
{
    public override void OnPlayerTakeDamage(Entity enemy)
    {
        enemy.AddStatusEffect(new StatusEffect(StatusType.DEATHMARK, 2));
    }
}
