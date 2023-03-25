using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverworldTown : OverworldNode
{
    [SerializeField] private List<ItemAndQuantity> ItemsAvailable;


    public List<Item> equipablesAvailableList = new List<Item>();
    public List<Item> consumablesAvailableList = new List<Item>();

    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();

        foreach (ItemAndQuantity item in ItemsAvailable)
        {
            if(item.data.itemType == ItemType.CONSUMABLE || item.data.itemType == ItemType.THROWABLE)
                consumablesAvailableList.Add(Inventory.CreateItem(item.data, item.quantity));

            else
                equipablesAvailableList.Add(Inventory.CreateItem(item.data, item.quantity));
        }
    }

    private void HighlightNode(bool active)
    {
        if (active)
            render.color = new Color(0.85f, 0.85f, 0.85f);
        else
            render.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (OverworldManager.instance.GetAllowSelection())
        {
            OverworldManager.instance.SetAllowSelection(false);
            OverworldManager.instance.MoveMiniPlayer(transform);
            TownManager.instance.EnterTown(this);
        }
    }

    private void OnMouseEnter()
    {
        if (OverworldManager.instance.GetAllowSelection())
            HighlightNode(true);
    }

    private void OnMouseExit()
    {
        HighlightNode(false);
    }
}
