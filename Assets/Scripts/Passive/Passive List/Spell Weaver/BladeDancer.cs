using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDancer : Passive
{
    private int playerMoveCounter = 0;

    public override void OnEndTurn(Entity entity)
    {
        playerMoveCounter = 0;
    }

    public override void OnPlayerMove(Player player)
    {
        playerMoveCounter++;
        player.ChangeMeleeDamage(data.passiveValue);
    }

    public override void OnPlayerAttack(Player player)
    {
        player.ChangeMeleeDamage(-(playerMoveCounter * data.passiveValue));
        playerMoveCounter = 0;
    }
}
