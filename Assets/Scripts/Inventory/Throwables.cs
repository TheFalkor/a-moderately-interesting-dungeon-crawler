using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Item
{
    public int throwableValue;
    public ThrowableType throwableType;
    private GameObject projectilePrefab;

    [Header("Runtime Variables")]
    private List<Tile> availableTiles = new List<Tile>();


    public Throwable(ThrowableItemSO data)
    {
        Initialize(data);

        throwableValue = data.throwableValue;
        throwableType = data.throwableType;
        projectilePrefab = data.projectilePrefab;
    }

    public override bool UseItem(Tile tile)
    {
        if (!availableTiles.Contains(tile))
            return false;

        switch (throwableType)
        {
            case ThrowableType.KUNAI:
                CombatManager.instance.StartCoroutine(ThrowProjectile(tile));
                break;
        }

        GridManager.instance.ClearAllHighlights();

        return true;
    }

    public override void HighlightDecision(Tile currentTile)
    {
        availableTiles.Clear();

        switch (throwableType)
        {
            case ThrowableType.KUNAI:
                foreach (Occupant o in CombatManager.instance.occupantList)
                {
                    if (Vector2.Distance(currentTile.GetPosition(), o.currentTile.GetPosition()) < 3.2f)
                    {
                        availableTiles.Add(o.currentTile);
                        o.currentTile.Highlight(HighlightType.ATTACKABLE);
                    }
                }

                foreach (Entity e in CombatManager.instance.entityList)
                {
                    if (e is Player)
                        continue;

                    if (Vector2.Distance(currentTile.GetPosition(), e.currentTile.GetPosition()) < 3.2f)
                    {
                        availableTiles.Add(e.currentTile);
                        e.currentTile.Highlight(HighlightType.ATTACKABLE);
                    }
                }
                break;
        }
    }

    IEnumerator ThrowProjectile(Tile target)
    {
        bool waitHappened = false;

        GameObject projectile = Object.Instantiate(projectilePrefab);
        projectile.transform.position = DungeonManager.instance.player.transform.position;
        projectile.transform.position += new Vector3(0, 0.5f, 0);
        projectile.GetComponent<Projectile>().setTarget(target.transform.position);


        while (!waitHappened)
        {
            waitHappened = true;
            yield return new WaitForSeconds(0.05f * Vector2.Distance(DungeonManager.instance.player.currentTile.GetPosition(), target.GetPosition()));
        }

        target.GetOccupant().TakeDamage(new Damage(throwableValue, DamageOrigin.FRIENDLY), DungeonManager.instance.player);
    }
}
