using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldDungeon : OverworldNode
{
    [SerializeField] private GameObject NodeLayout;
    private SpriteRenderer render;
    public bool completed = false;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public void MarkCompleted()
    { 
        completed = true;
        render.color = new Color(0.50f, 0.50f, 0.50f);
    }

    private void HighlightNode(bool active)
    {
        if (active && !completed)
            render.color = new Color(0.85f, 0.85f, 0.85f);
        else
            render.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (completed)
            return;

        if (!OverworldManager.instance.GetAllowSelection())
            return;

        MarkCompleted();
        OverworldManager.instance.EnterDungeon(NodeLayout);
        OverworldManager.instance.MoveMiniPlayer(transform);
    }

    private void OnMouseEnter()
    {
        if (completed)
            return;

        if (OverworldManager.instance.GetAllowSelection())
            HighlightNode(true);
    }

    private void OnMouseExit()
    {
        if (completed)
            return;

        HighlightNode(false);
    }
}
