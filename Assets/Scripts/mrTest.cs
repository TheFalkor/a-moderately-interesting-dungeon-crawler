using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mrTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TilePathfinding mrPathfinder = gameObject.GetComponent<TilePathfinding>();

        Tile mr1 = GridManager.instance.GetTile(new Vector2Int(0, 0));
        Tile mr2 = GridManager.instance.GetTile(new Vector2Int(9, 6));

        Debug.Log(mr1.GetPosition());
        Debug.Log(mr2.GetPosition());

        mrPathfinder.CreatePath(mr1, mr2);
    }
}
