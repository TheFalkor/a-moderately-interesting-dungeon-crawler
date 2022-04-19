using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tilePrefab;
    [Space]
    public bool useRoomPreset;
    public CombatRoomSO room;

    [Header("Grid Settings")]
    private const int RANDOM_WALL_CHANCE = 10;
    private const int ROOM_WIDTH = 10;
    private const int ROOM_HEIGHT = 7;

    [Header("Runtime Variables")]
    private readonly List<Tile> tileList = new List<Tile>();

    [Header("Singleton")]
    public static GridManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        CreateRoom();
    }

    private void CreateRoom()
    {
        GameObject parent = new GameObject("Room");
        for (int i = 0; i < ROOM_WIDTH * ROOM_HEIGHT; i++)
        {
            Tile temp = Instantiate(tilePrefab, new Vector2(-ROOM_WIDTH / 2.0f + i % ROOM_WIDTH + 0.5f, ROOM_HEIGHT / 2.0f - i / ROOM_WIDTH), Quaternion.identity, parent.transform).GetComponentInParent<Tile>();

            if (useRoomPreset)
            {
                temp.Initialize(new Vector2Int(i % ROOM_WIDTH, i / ROOM_WIDTH), !room.tiles[i].wall);
            }
            else
            {
                if (Random.Range(0, 100) < RANDOM_WALL_CHANCE)
                    temp.Initialize(new Vector2Int(i % 10, i / 10), false);
                else
                    temp.Initialize(new Vector2Int(i % 10, i / 10), true);

            }

            tileList.Add(temp);
        }

        for (int i = 0; i < tileList.Count; i++)
        {
            Tile tile = tileList[i];

            if (i / ROOM_WIDTH != 0)
            {
                if (i % ROOM_WIDTH != 0)
                    tile.diagonalNeighbors.Add(tileList[i - ROOM_WIDTH - 1]);    // NW

                tile.orthogonalNeighbors.Add(tileList[i - ROOM_WIDTH]);            // N

                if (i % ROOM_WIDTH != ROOM_WIDTH - 1)
                    tile.diagonalNeighbors.Add(tileList[i - ROOM_WIDTH + 1]);    // NE
            }

            if (i % ROOM_WIDTH != ROOM_WIDTH - 1)
                tile.orthogonalNeighbors.Add(tileList[i + 1]);                    // E

            if (i / ROOM_WIDTH != ROOM_HEIGHT - 1)
            {
                if (i % ROOM_WIDTH != ROOM_WIDTH - 1)
                    tile.diagonalNeighbors.Add(tileList[i + ROOM_WIDTH + 1]);    // SE

                tile.orthogonalNeighbors.Add(tileList[i + ROOM_WIDTH]);            //S

                if (i % ROOM_WIDTH != 0)
                    tile.diagonalNeighbors.Add(tileList[i + ROOM_WIDTH - 1]);    // SW
            }

            if (i % ROOM_WIDTH != 0)
                tile.orthogonalNeighbors.Add(tileList[i - 1]);                    // W
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        if (position.x < 0 || position.x >= ROOM_WIDTH || position.y < 0 || position.y >= ROOM_HEIGHT)
            return null;

        int index = position.x + position.y * ROOM_WIDTH;
        return tileList[index];
    }

    public Tile GetTileWorld(Vector2 position)
    {
        if (position.x < -ROOM_WIDTH / 2.0f || position.x > ROOM_WIDTH / 2.0f || position.y < -ROOM_HEIGHT / 2.0f || position.y > ROOM_HEIGHT / 2.0f)
            return null;

        int index = (int)(ROOM_WIDTH / 2.0f + position.x + (ROOM_HEIGHT / 2.0f - position.y) * ROOM_WIDTH);
        return tileList[index];
    }
    public Tile GetTileWorldFuzzy(Vector2 position) 
    {
        const float widthOffset = 0.5f;
        const float hightOffset = 0;
        const float tileWidth = 1;
        const float tileHeight = 1;
        Vector2 startOfGrid = new Vector2(-(ROOM_WIDTH / 2f +tileWidth/2f)+widthOffset, (ROOM_HEIGHT / 2f  + tileHeight / 2f) +hightOffset);
        Vector2 diffrence = position - startOfGrid;
        Vector2Int gridPosition = new Vector2Int((int)(diffrence.x/tileWidth), -(int)(diffrence.y/tileHeight));
        //need to invert y because top left is  X negative : Y positive in world and X negative :Y negative in get tile
        return GetTile(gridPosition);
    }
}