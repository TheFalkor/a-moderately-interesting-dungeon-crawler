using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnEnemyDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath(Entity enemy)
    {
        if (!(enemy is Enemy dead))
            return;

        int overkillDamage = enemy.GetCurrentHealth() * -1;

        Enemy target = null;

        if (overkillDamage > 0)
        {
            foreach (Entity e in CombatManager.instance.entityList)
            {
                if (!(e is Enemy))
                    continue;

                if (e.GetCurrentHealth() < 0)
                    continue;

                if (!target)
                    target = (Enemy)e;
                else if (Vector2.Distance(e.currentTile.GetPosition(), dead.currentTile.GetPosition()) < Vector2.Distance(target.currentTile.GetPosition(), dead.currentTile.GetPosition()))
                    target = (Enemy)e;
            }

            if (target)
            {
                GameObject dyingVFX = Object.Instantiate(data.passiveVFX[0], enemy.transform.position, Quaternion.identity);
                Object.Destroy(dyingVFX, 6 / 15f);

                GameObject receiveVFX = Object.Instantiate(data.passiveVFX[2], target.transform.position, Quaternion.identity);
                Object.Destroy(receiveVFX, 3 / 15f);

                target.TakeCleanDamage(overkillDamage, DamageOrigin.FRIENDLY);
            }
        }
    }
}
