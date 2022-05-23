using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMP : Passive
{
    public override void OnPreTurn(Entity entity)
    {
        if (entity is Player player)
        {
            player.ChangeCurrentMP(data.passiveValue);
        }
    }
}
