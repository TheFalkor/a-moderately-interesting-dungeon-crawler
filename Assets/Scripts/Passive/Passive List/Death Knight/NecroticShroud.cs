using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroticShroud : Passive
{
    public override void OnPlayerTakeDamage(Entity enemy)
    {
        Debug.Log(enemy.transform.name);
        enemy.AddStatusEffect(new StatusEffect(StatusType.DEATHMARK, 2));
    }
}
