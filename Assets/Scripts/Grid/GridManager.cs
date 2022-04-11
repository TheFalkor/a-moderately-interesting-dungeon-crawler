using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;

    private const int TEMP_WALL_CHANCE = 10;
    private const int ROOM_WIDTH = 10;
    private const int ROOM_HEIGHT = 7;

    private List<Tile> tileList = new List<Tile>();
        

    void Start()
    {
        GameObject parent = new GameObject("Map");
        for (int i = 0; i < ROOM_WIDTH * ROOM_HEIGHT; i++)
        {
            Tile temp = Instantiate(tilePrefab, new Vector2(-ROOM_WIDTH / 2.0f + i % ROOM_WIDTH + 0.5f, ROOM_HEIGHT / 2.0f - i / ROOM_WIDTH), Quaternion.identity, parent.transform).GetComponentInParent<Tile>();
            
            if (Random.Range(0, 100) < TEMP_WALL_CHANCE)
                temp.Initialize(new Vector2Int(i % 10, i / 10), false);
            else
                temp.Initialize(new Vector2Int(i % 10, i / 10), true);

            tileList.Add(temp);
        }

    }


    public Tile GetTile(Vector2Int position)
    {
        if (position.x < 0 || position.x >= ROOM_WIDTH || position.y < 0 || position.y >= ROOM_HEIGHT)
            return null;

        int index = position.x + position.y * ROOM_HEIGHT;
        return tileList[index];
    }

    public Tile GetTileWorld(Vector2 position)
    {
        if (position.x < -ROOM_WIDTH / 2.0f || position.x > ROOM_WIDTH / 2.0f || position.y < -ROOM_HEIGHT / 2.0f || position.y > ROOM_HEIGHT / 2.0f)
            return null;

        int index = (int)(ROOM_WIDTH / 2.0f + position.x + (ROOM_HEIGHT / 2.0f - position.y) * ROOM_WIDTH);
        return tileList[index];
    }
}