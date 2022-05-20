using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedGrounds : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> corruptedTiles = new List<Tile>();

    [Header("References")]
    private Player player;


    public override bool UseAbility(Tile tile)
    {
        if (!corruptedTiles.Contains(tile))
            return false;

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

        corruptedTiles.Add(currentTile);
    }

    public override bool Tick(float deltaTime)
    {
        foreach (Tile tile in corruptedTiles)
        {
            if (!tile.IsWalkable())
                continue;

            TileEffect effect = AbilityManager.instance.SpawnTileEffect(data.abilityPrefab);
            effect.transform.position = tile.transform.position;
            effect.Initialize(Mathf.RoundToInt(data.abilityValue));
        }

        return true;
    }
}
