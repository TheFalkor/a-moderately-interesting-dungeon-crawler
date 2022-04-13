using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JustTestingThingsScript : MonoBehaviour
{
    public GridManager mrGrid;
    float tock;
    // Start is called before the first frame update
    void Start()
    {
       
        tock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tock += Time.deltaTime;
        if (tock > 1) 
        {
            mrGrid.GetTileWorld(new Vector2(0, 0)).Highlight(false);
            tock = 0;
        }
    }
}
