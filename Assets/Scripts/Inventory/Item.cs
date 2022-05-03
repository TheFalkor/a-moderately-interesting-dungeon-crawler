using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemType itemType;
    [Space]
    public int count;
    public int maxStack;


    public void Initialize(ItemSO data)
    {
        itemName = data.itemName;
        itemDescription = data.itemDescription;
        itemSprite = data.itemSprite;
        itemType = data.itemType;

        maxStack = data.maxStack;
    }

    public virtual bool UseItem()
    {
        Debug.Log("UseItem() not implemented.");
        return false;
    }

    public virtual bool UseItem(Tile tile)
    {
        Debug.Log("UseItem(Tile) not implemented.");
        return false;
    }

    public virtual void HighlightDecision(Tile currentTile)
    {
        Debug.Log("HighlightDecision() not implemented.");
    }
}
