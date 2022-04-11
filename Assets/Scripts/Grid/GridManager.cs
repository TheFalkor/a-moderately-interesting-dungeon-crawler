using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;

    private const int TEMP_WALL_CHANCE = 10;
    private int width = 10;
    private int height = 10;

    private List<Tile> tileList = new List<Tile>();
        

    void Start()
    {
        GameObject parent = new GameObject("Map");
        for (int i = 0; i < 10 * 10; i++)
        {
            Tile temp = Instantiate(tilePrefab, new Vector2(-5 + i % 10 + 0.5f, 5 - i / 10), Quaternion.identity, parent.transform).GetComponentInParent<Tile>();
            
            if (Random.Range(0, 100) < TEMP_WALL_CHANCE)
                temp.Initialize(new Vector2Int(i % 10, i / 10), false);
            else
                temp.Initialize(new Vector2Int(i % 10, i / 10), true);

            tileList.Add(temp);
        }

    }


    public Tile GetTile(Vector2Int pos)
    {
        int index = pos.x + pos.y * height;

        return tileList[index];
    }
}