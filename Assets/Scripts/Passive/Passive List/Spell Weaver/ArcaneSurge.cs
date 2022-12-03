using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSurge : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnEnemyDeath += OnEnemyDeath;
    }
    private void OnEnemyDeath(Entity enemy)
    {
        GameObject orb = Object.Instantiate(data.tileEffect, enemy.transform.position, Quaternion.identity);
        orb.GetComponent<TileEffect>().Initialize(99);
    }
}
