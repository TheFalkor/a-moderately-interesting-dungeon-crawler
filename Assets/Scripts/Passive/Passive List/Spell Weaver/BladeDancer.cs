using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDancer : Passive
{
    private int playerMoveCounter = 0;


    public override void Initialize()
    {
        ServiceLocator.Get<EventManager>().OnEndTurn += OnEndTurn;
        ServiceLocator.Get<EventManager>().OnPlayerMove += OnPlayerMove;
        ServiceLocator.Get<EventManager>().OnPlayerAttack += OnPlayerAttack;
    }
    private void OnEndTurn(Entity entity)
    {
        playerMoveCounter = 0;
    }

    private void OnPlayerMove(Player player)
    {
        playerMoveCounter++;
        player.ChangeMeleeDamage(data.passiveValue);
    }

    private void OnPlayerAttack(Player player)
    {
        player.ChangeMeleeDamage(-(playerMoveCounter * data.passiveValue));
        playerMoveCounter = 0;
    }
}
