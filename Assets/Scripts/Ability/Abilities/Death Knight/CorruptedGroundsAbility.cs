using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedGroundsAbility : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> corruptedTiles = new List<Tile>();
    private float animationTimer = 0;

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (!corruptedTiles.Contains(tile))
            return false;

        animationTimer = 1f;

        return true;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
        corruptedTiles.Clear();

        player = (Player)currentTile.GetOccupant();

        corruptedTiles.AddRange(currentTile.orthogonalNeighbors);
        corruptedTiles.AddRange(currentTile.diagonalNeighbors);

        foreach (Tile tile in corruptedTiles)
            tile.Highlight(HighlightType.ABILITY_TARGET);
    }

    public override bool Tick(float deltaTime)
    {
        if (animationTimer > 0)
        {
            animationTimer -= deltaTime;

            player.transform.GetChild(0).transform.localScale = new Vector3(1, 0.5f + Mathf.Sin(animationTimer * 30) * 0.5f, 1);
        }
        else
        {
            player.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);

            // Spawn death pool
            return true;
        }

        return false;
    }
}
