using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMP : Passive
{
    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnPreTurn += OnPreTurn;
    }

    private void OnPreTurn(Entity entity)
    {
        if (entity is Player player)
        {
            player.ChangeCurrentMP(data.passiveValue);
        }
    }
}
