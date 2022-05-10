using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutterSpread : Passive
{
    public override void OnEnemyTakeDamage(Entity entity)
    {
        foreach (StatusEffect effect in entity.activeStatusEffects)
        {
            if (effect.type == StatusType.FLUTTER)
            {
                foreach (Tile tile in entity.currentTile.orthogonalNeighbors)
                {
                    Occupant occ = tile.GetOccupant();

                    if (occ && occ.originType == DamageOrigin.ENEMY)
                        occ.AddStatusEffect(new StatusEffect(StatusType.FLUTTER, -1));
                }
            }
        }
    }
}
