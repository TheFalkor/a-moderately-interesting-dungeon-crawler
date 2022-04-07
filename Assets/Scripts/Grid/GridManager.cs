using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;

    private const int WALL_EREASD = 10;
        
    void Start()
    {
        GameObject parent = new GameObject("Map");
        for (int i = 0; i < 10 * 10; i++)
        {
            Tile temp = Instantiate(tilePrefab, new Vector2(-5 + i % 10 + 0.5f, 5 - i / 10), Quaternion.identity, parent.transform).GetComponentInParent<Tile>();

            if (Random.Range(0, 100) < WALL_EREASD)
                temp.Initialize(new Vector2Int(i % 10, i / 10), true);
            else
                temp.Initialize(new Vector2Int(i % 10, i / 10), false);
        }
    }

}
