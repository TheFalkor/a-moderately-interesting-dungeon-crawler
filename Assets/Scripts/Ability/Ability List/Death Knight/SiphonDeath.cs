using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonDeath : Ability
{
    [Header("Runtime Variables")]
    private Tile currentTile;
    private float animationTimer = 0;
    private int markCount = 0;

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (currentTile != tile)
            return false;

        animationTimer = 0.25f;
        markCount = 0;
        
        return true;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
        this.currentTile = currentTile;

        player = (Player)currentTile.GetOccupant();

        currentTile.Highlight(HighlightType.ABILITY_TARGET);
    }

    public override bool Tick(float deltaTime)
    {
        foreach (Entity entity in CombatManager.instance.entityList)
        {
            foreach (StatusEffect effect in entity.activeStatusEffects)
            {
                if (effect.type == StatusType.DEATHMARK)
                {
                    markCount++;

                    GameObject temp = Object.Instantiate(data.abilityVFX[1], entity.transform.position, Quaternion.identity);
                    Object.Destroy(temp, 6 / 15f);

                    entity.activeStatusEffects.Remove(effect);
                    break;
                }
            }
        }

        animationTimer -= deltaTime;

        if (animationTimer <= 0)
        { 
            if (markCount > 0)
            {
                GameObject tempSelf = Object.Instantiate(data.abilityVFX[0], player.transform.position, Quaternion.identity);
                tempSelf.transform.parent = player.transform;
            }

            DungeonManager.instance.player.AddShield(markCount * Mathf.RoundToInt(data.abilityValue));
            return true;
        }

        return false;
    }
}
