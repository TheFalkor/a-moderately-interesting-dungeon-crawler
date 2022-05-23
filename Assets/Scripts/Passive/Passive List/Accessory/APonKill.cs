using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APonKill : Passive
{
    public override void OnEnemyDeath(Entity enemy)
    {
        DungeonManager.instance.player.ChangeCurrentAP(data.passiveValue);
    }
}
