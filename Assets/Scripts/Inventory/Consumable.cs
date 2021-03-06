using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int consumableValue;
    public ConsumableType consumableType;

    [Header("Runtime Variables")]
    private List<Tile> availableTiles = new List<Tile>();


    public Consumable(ConsumableItemSO data)
    {
        Initialize(data);

        consumableValue = data.consumableValue;
        consumableType = data.consumableType;
    }

    public override bool UseItem()
    {
        switch (consumableType)
        {
            case ConsumableType.HEALING:
                DungeonManager.instance.player.Heal(consumableValue);
                break;
        }

        return true;
    }

    public override bool UseItem(Tile tile)
    {
        if (!availableTiles.Contains(tile))
            return false;

        switch (consumableType)
        {
            case ConsumableType.HEALING:
                tile.GetOccupant().Heal(consumableValue);
                break;
        }

        GridManager.instance.ClearAllHighlights();

        return true;
    }

    public override void HighlightDecision(Tile currentTile)
    {
        availableTiles.Clear();

        switch (consumableType)
        {
            case ConsumableType.HEALING:
                availableTiles.Add(currentTile);
                currentTile.Highlight(HighlightType.HEALABLE);
                break;
        }
    }
}
