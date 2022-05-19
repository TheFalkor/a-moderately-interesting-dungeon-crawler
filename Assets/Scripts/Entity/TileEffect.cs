using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    [Header("Runtime Variables")]
    protected SpriteRenderer render;
    [HideInInspector] public Tile currentTile;
    [HideInInspector] public int duration;


    public virtual void Initialize(int duration)
    {
        this.duration = duration;

        currentTile = GridManager.instance.GetTileWorld(transform.position);
        currentTile.SetTileEffect(this);

        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        render.sortingOrder = currentTile.GetPosition().y * 5 - 90 + 1;

        CombatManager.instance.AddTileEffect(this);
    }

    public virtual void OnSpawn() 
    {
        
    }

    public virtual void PreTurn() 
    {
        duration -= 1;

        if (duration == 0)
            OnDespawn();
    }

    public virtual void OnDespawn() 
    {
        CombatManager.instance.RemoveTileEffect(this);
        currentTile.SetTileEffect(null);
        Destroy(gameObject);
    }
}
