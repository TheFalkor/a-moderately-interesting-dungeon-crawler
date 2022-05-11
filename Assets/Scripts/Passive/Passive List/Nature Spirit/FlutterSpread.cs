using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutterSpread : Passive
{
    public override void OnEnemyTakeDamage(Entity enemy)
    {
        for (int i = 0; i < enemy.activeStatusEffects.Count; i++)
        { 
            StatusEffect effect = enemy.activeStatusEffects[i];

            if (effect.type == StatusType.FLUTTER)
            {
                foreach (Tile tile in enemy.currentTile.orthogonalNeighbors)
                {
                    Occupant occ = tile.GetOccupant();

                    if (occ && occ.originType == DamageOrigin.ENEMY)
                        occ.AddStatusEffect(new StatusEffect(StatusType.FLUTTER, -1));
                }
            }
        }
    }
}
