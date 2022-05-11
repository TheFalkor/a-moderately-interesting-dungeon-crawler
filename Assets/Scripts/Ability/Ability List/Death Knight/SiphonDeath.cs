using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonDeath : Ability
{
    [Header("Runtime Variables")]
    private Tile currentTile;
    private float animationTimer = 0;

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (currentTile != tile)
            return false;

        animationTimer = 1f;
        
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
        if (animationTimer > 0)
        {
            animationTimer -= deltaTime;

            Color color = player.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            color.a = 1 - Mathf.Cos((animationTimer - 0.5f) * Mathf.Deg2Rad * 180);
            player.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            return true;
        }

        return false;
    }
}
