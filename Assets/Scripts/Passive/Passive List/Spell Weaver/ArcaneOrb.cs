using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneOrb : TileEffect
{
    public override void Initialize(int duration)
    {
        base.Initialize(duration);

        Player.playerMove += PlayerMoved;
    }

    public void PlayerMoved(Tile t)
    {
        if (t == currentTile)
        {
            AbilityManager.instance.DecreaseAbilityCooldown();

            CombatManager.instance.RemoveTileEffect(this);
            Player.playerMove -= PlayerMoved;
            Destroy(gameObject, 0.1f);
        }
    }

    public override void OnDespawn()
    {
        Player.playerMove -= PlayerMoved;

        base.OnDespawn();
    }
}
