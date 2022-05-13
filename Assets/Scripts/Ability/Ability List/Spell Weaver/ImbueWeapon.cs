using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImbueWeapon : Ability
{
    bool imbueActive = false;

    [Header("References")]
    private Player player;

    public override void HighlightDecisions(Tile currentTile)
    {
        if (!player)
            player = (Player)currentTile.GetOccupant();

        Player.playerAttack += PlayerAttacked;
        Player.playerEndTurn += PlayerTurnEnd;

        currentTile.Highlight(HighlightType.ABILITY_TARGET);
    }

    public override bool Tick(float deltaTime)
    {
        return true;
    }

    public override bool UseAbility(Tile tile)
    {
        if (imbueActive)
            return false;

        if (tile == player.currentTile)
        {
            player.ChangeMeleeDamage(data.abilityValue);
            GameObject temp = Object.Instantiate(data.abilityVFX[0], player.transform.position, Quaternion.identity);
            Object.Destroy(temp, 8 / 15f);
            imbueActive = true;
            return true;
        }
            
        else
            return false;
    }

    private void PlayerAttacked()
    {
        if (imbueActive)
        {
            player.ChangeMeleeDamage(data.abilityValue * -1);
            imbueActive = false;
        }
    }

    private void PlayerTurnEnd()
    {
        if (imbueActive)
        {
            player.ChangeMeleeDamage(data.abilityValue * -1);
            imbueActive = false;
        }
    }
}
