using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedPool : TileEffect
{
    public override void Initialize(int duration)
    {
        base.Initialize(duration);

        OnSpawn();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        ApplyMark();
    }

    public override void PreTurn()
    {
        base.PreTurn();

        if (duration == 0)
            return;

        ApplyMark();
    }

    public override void OnDespawn()
    {
        ApplyMark();
        base.OnDespawn();
    }

    private void ApplyMark()
    {
        Occupant occ = currentTile.GetOccupant();

        if (occ)
        {
            if (occ.originType == DamageOrigin.ENEMY)
                occ.AddStatusEffect(new StatusEffect(StatusType.DEATHMARK, -1));
        }

    }
}
