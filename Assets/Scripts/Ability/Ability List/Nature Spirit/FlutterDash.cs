using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlutterDash : Ability
{
    [Header("Runtime Variables")]
    private List<Tile> dashableTiles = new List<Tile>();
    private Tile targetTile;
    private float animationTimer = 0;
    private int dashState = 0;
    private bool verticalDash = false;

    [Header("References")]
    private Player player;
    private Animator playerAnimator;


    public override bool UseAbility(Tile tile)
    {
        if (!dashableTiles.Contains(tile))
            return false;

        affectedEnemies.Clear();

        Vector2Int direction = tile.GetPosition() - player.currentTile.GetPosition();
        int dashedTiles = (int)direction.magnitude;
        direction.x /= dashedTiles;
        direction.y /= dashedTiles;

        for (int i = 1; i < dashedTiles; i++)
        {
            Occupant occ = GridManager.instance.GetTile(player.currentTile.GetPosition() + direction * i).GetOccupant();
            if (occ && occ.originType == DamageOrigin.ENEMY)
                affectedEnemies.Add((Entity)occ);
        }


        if (tile.transform.position.x > player.transform.position.x)
            player.transform.localScale = new Vector3(-1, 1);
        else
            player.transform.localScale = new Vector3(1, 1);

        targetTile = tile;

        verticalDash = player.transform.position.x == targetTile.transform.position.x;

        animationTimer = 0;
        dashState = 0;
        playerAnimator.Play("Dash Start");

        return true;
    }

    public override void HighlightDecisions(Tile currentTile)
    {
        dashableTiles.Clear();

        if (player == null)
        {
            player = (Player)currentTile.GetOccupant();
            playerAnimator = player.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }

        Queue<Vector2Int> directionQueue = new Queue<Vector2Int>();

        directionQueue.Enqueue(new Vector2Int(0, -1));
        directionQueue.Enqueue(new Vector2Int(1, 0));
        directionQueue.Enqueue(new Vector2Int(0, 1));
        directionQueue.Enqueue(new Vector2Int(-1, 0));

        while (directionQueue.Count != 0)
        {
            Tile tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek());
            if (!tile)
            {
                directionQueue.Dequeue();
                continue;
            }

            tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek() * 2);
            if (!tile)
            {
                directionQueue.Dequeue();
                continue;
            }

            if (tile.IsWalkable() && !tile.IsOccupied())
                dashableTiles.Add(tile);

            tile.Highlight(HighlightType.ABILITY_TARGET, false);

            tile = GridManager.instance.GetTile(currentTile.GetPosition() + directionQueue.Peek() * 3);
            if (!tile)
            {
                directionQueue.Dequeue();
                continue;
            }

            if (!tile.IsWalkable() || tile.IsOccupied())
            {
                directionQueue.Dequeue();
                tile.Highlight(HighlightType.ABILITY_TARGET, false);
                continue;
            }

            directionQueue.Dequeue();
            dashableTiles.Add(tile);

            tile.Highlight(HighlightType.ABILITY_TARGET);
        }
    }

    public override bool Tick(float deltaTime)
    {
        animationTimer += deltaTime;

        if (dashState == 0)     // Dash Start
        {
            if (animationTimer >= 24 / 60f)
            {
                if (verticalDash)
                {
                    int factor = player.transform.position.y > targetTile.transform.position.y? 1: -1;
                    player.transform.GetChild(0).localPosition = new Vector3(0.75f * factor, 1f, 0);
                    player.transform.GetChild(0).eulerAngles = new Vector3(0, 0, 90 * factor);
                }

                dashState = 1;
                playerAnimator.Play("Dash Middle");
            }
        }
        else if (dashState == 1)    // Dash Middle
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetTile.transform.position, deltaTime * 8);
        
            if (targetTile.transform.position == player.transform.position)
            {
                player.currentTile.SetOccupant(null);
                player.currentTile = targetTile;
                player.currentTile.SetOccupant(player);

                player.UpdateLayerIndex();

                foreach (Occupant occupant in affectedEnemies)
                {
                    occupant.AddStatusEffect(new StatusEffect(StatusType.FLUTTER, 1));
                }

                if (verticalDash)
                {
                    player.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
                    player.transform.GetChild(0).eulerAngles = new Vector3(0, 0, 0);
                }

                playerAnimator.Play("Dash End");
                dashState = 2;
                animationTimer = 0;
            }
        }
        else    // Dash End
        {
            if (animationTimer >= 21 / 60f)
            {
                return true;
            }
        }

       
        return false;
    }
}
