using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneTransference : Passive
{
    public override void OnEnemyDeath(Entity enemy)
    {
        GameObject orb = Object.Instantiate(data.tileEffect, enemy.transform.position, Quaternion.identity);
        orb.GetComponent<TileEffect>().Initialize(99);
    }
}
