using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornDash : Passive
{
    public override void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        if(ability == AbilityID.FLUTTER_DASH)
        {
            foreach (Entity entity in affectedEnemies)
            {
                entity.TakePureDamage(5, DamageOrigin.FRIENDLY);
            }
        }
    }
}
