using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesetManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D tileset;

    [Header("Runtime Variables")]
    private readonly List<int> wallIndeces = new List<int>();
    private readonly List<int> groundIndeces = new List<int>();

    private readonly List<Sprite> spriteList = new List<Sprite>();

    [Header("Singleton")]
    public static TilesetManager instance;


    void Awake()
    {
        if (instance)
            return;

        instance = this;


        wallIndeces.Add(4);
        wallIndeces.Add(5);
        wallIndeces.Add(6);
        wallIndeces.Add(7);
        wallIndeces.Add(12);
        wallIndeces.Add(13);
        wallIndeces.Add(14);
        wallIndeces.Add(15);
        wallIndeces.Add(20);
        wallIndeces.Add(21);
        wallIndeces.Add(22);
        wallIndeces.Add(23);
        wallIndeces.Add(28);
        wallIndeces.Add(29);
        wallIndeces.Add(30);
        wallIndeces.Add(31);

        groundIndeces.Add(3);
        groundIndeces.Add(11);
        groundIndeces.Add(19);
        groundIndeces.Add(27);
        groundIndeces.Add(8);
        groundIndeces.Add(10);
        groundIndeces.Add(17);
        groundIndeces.Add(32);
        groundIndeces.Add(33);
        groundIndeces.Add(40);
        groundIndeces.Add(41);
        

        for (int i = 0; i < 64; i++)
            spriteList.Add(CutSprite(i));
    }

    public void CalculateWallTile(Tile tile)
    {
        List<int> availableSprites = new List<int>();
        availableSprites.AddRange(wallIndeces);

        bool wallLeft = false;
        bool wallRight = false;

        /*Tile downDownTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.up * 2);
        if (downDownTile && downDownTile.IsWalkable())
        {

        }*/

        Tile upTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.down);
        if (upTile && upTile.IsWalkable())    // No wall above
        {
            availableSprites.Remove(12);
            availableSprites.Remove(13);
            availableSprites.Remove(14);
            availableSprites.Remove(15);

            availableSprites.Remove(20);
            availableSprites.Remove(21);
            availableSprites.Remove(22);
            availableSprites.Remove(23);
        }
        else
        {
            availableSprites.Remove(4);
            availableSprites.Remove(5);
            availableSprites.Remove(6);
            availableSprites.Remove(7);
            availableSprites.Remove(28);
            availableSprites.Remove(29);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }

        Tile downTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.up);
        if (downTile && downTile.IsWalkable())      // No wall below
        {
            availableSprites.Remove(4);
            availableSprites.Remove(5);
            availableSprites.Remove(6);
            availableSprites.Remove(7);

            availableSprites.Remove(12);
            availableSprites.Remove(13);
            availableSprites.Remove(14);
            availableSprites.Remove(15);
        }
        else
        {
            availableSprites.Remove(20);
            availableSprites.Remove(21);
            availableSprites.Remove(22);
            availableSprites.Remove(23);
            availableSprites.Remove(28);
            availableSprites.Remove(29);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }

        Tile leftTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.left);
        if (leftTile && leftTile.IsWalkable())  // No wall left
        {
            availableSprites.Remove(5);
            availableSprites.Remove(6);
            availableSprites.Remove(13);
            availableSprites.Remove(14);
            availableSprites.Remove(21);
            availableSprites.Remove(22);
            availableSprites.Remove(29);
            availableSprites.Remove(30);
        }
        else
        {
            wallLeft = true;

            availableSprites.Remove(4);
            availableSprites.Remove(7);
            availableSprites.Remove(12);
            availableSprites.Remove(15);
            availableSprites.Remove(20);
            availableSprites.Remove(23);
            availableSprites.Remove(28);
            availableSprites.Remove(31);
        }

        Tile rightTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.right);
        if (rightTile && rightTile.IsWalkable())    // No wall right
        {
            availableSprites.Remove(4);
            availableSprites.Remove(5);
            availableSprites.Remove(12);
            availableSprites.Remove(13);
            availableSprites.Remove(20);
            availableSprites.Remove(21);
            availableSprites.Remove(28);
            availableSprites.Remove(29);
        }
        else
        {
            wallRight = true;

            availableSprites.Remove(6);
            availableSprites.Remove(7);
            availableSprites.Remove(14);
            availableSprites.Remove(15);
            availableSprites.Remove(22);
            availableSprites.Remove(23);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }

        if (wallLeft != wallRight)
        {
        }


        if (availableSprites.Count != 1)
        {
            Debug.Log(tile.GetPosition() + " : " + availableSprites.Count);
            foreach (int i in availableSprites)
                Debug.Log(i);
        }    

        if (availableSprites.Count == 1)
            tile.GetComponent<SpriteRenderer>().sprite = spriteList[availableSprites[0]];
        else
            tile.GetComponent<SpriteRenderer>().sprite = null;

    }

    public void CalculateFloorTile(Tile tile)
    {
        List<int> availableSprites = new List<int>();

        availableSprites.Add(3);
        availableSprites.Add(3);
        availableSprites.Add(3);
        availableSprites.Add(3);
        availableSprites.Add(3);
        availableSprites.Add(11);
        availableSprites.Add(11);
        availableSprites.Add(11);
        availableSprites.Add(19);
        availableSprites.Add(19);
        availableSprites.Add(19);
        availableSprites.Add(19);
        availableSprites.Add(27);
        availableSprites.Add(27);

        tile.GetComponent<SpriteRenderer>().sprite = spriteList[availableSprites[Random.Range(0, availableSprites.Count)]];

        return;
        availableSprites.AddRange(groundIndeces);

        bool wallleft = false;
        bool wallRight = false;
        bool wallTL = false;
        bool wallTR = false;


        Tile upLeftTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.down + Vector2Int.left);
        if (upLeftTile && upLeftTile.IsWalkable())
        {
            availableSprites.Remove(6);
        }
        else
            wallTL = true;

        Tile upRightTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.down + Vector2Int.right);
        if (upRightTile && upRightTile.IsWalkable())
        {
            availableSprites.Remove(4);
        }
        else
            wallTR = true;

        Tile upTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.down);
        if (!upTile || !upTile.IsWalkable())
        {
            availableSprites.Remove(3);
            availableSprites.Remove(7);
            availableSprites.Remove(11);
            availableSprites.Remove(15);

            availableSprites.Remove(4);
            availableSprites.Remove(6);
            availableSprites.Remove(20);
            availableSprites.Remove(21);
        }
        else
        {
            availableSprites.Remove(9);
            if (!wallTR)
                availableSprites.Remove(16);
            else
                availableSprites.Remove(20);

            if (!wallTL)
                availableSprites.Remove(17);
            else
                availableSprites.Remove(21);
        }

        Tile leftTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.left);
        if (!leftTile || !leftTile.IsWalkable())
        {
            wallleft = true;

            availableSprites.Remove(3);
            availableSprites.Remove(7);
            availableSprites.Remove(11);
            availableSprites.Remove(4);
            availableSprites.Remove(6);
            availableSprites.Remove(9);
            availableSprites.Remove(17);
            availableSprites.Remove(21);
        }
        else
        {
            availableSprites.Remove(16);
            availableSprites.Remove(20);
        }

        Tile rightTile = GridManager.instance.GetTile(tile.GetPosition() + Vector2Int.right);
        if (!rightTile || !rightTile.IsWalkable())
        {
            wallRight = true;

            availableSprites.Remove(3);
            availableSprites.Remove(7);
            availableSprites.Remove(11);
            availableSprites.Remove(4);
            availableSprites.Remove(6);
            availableSprites.Remove(9);
            availableSprites.Remove(16);
            availableSprites.Remove(20);
        }
        else
        {
            availableSprites.Remove(17);
            availableSprites.Remove(21);
        }

        if (wallleft != wallRight)
        {
            availableSprites.Remove(15);
        }

        if (!wallRight && !wallRight)
        {
            availableSprites.Remove(15);
        }

        if (wallTL || wallTR)
        {
            availableSprites.Remove(3);
            availableSprites.Remove(7);
            availableSprites.Remove(11);
        }


        if (availableSprites.Count != 1 && availableSprites.Count != 3)
        {
            Debug.Log(tile.GetPosition() + " :: " + availableSprites.Count);

            foreach (int i in availableSprites)
                Debug.Log(i);
        }

        if (availableSprites.Count == 1)
            tile.GetComponent<SpriteRenderer>().sprite = spriteList[availableSprites[0]];
        else if (availableSprites.Count > 0)
            tile.GetComponent<SpriteRenderer>().sprite = spriteList[availableSprites[Random.Range(0, 3)]];
        else
            tile.GetComponent<SpriteRenderer>().sprite = null;
    }

    public Sprite GetTileSprite(int index)
    {
        return spriteList[index];
    }

    public Sprite GetBorderSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.EAST: return spriteList[18];
            case Direction.SOUTH: return spriteList[25];
            case Direction.WEST: return spriteList[16];
            case Direction.NORTH_EAST: return spriteList[2];
            case Direction.SOUTH_EAST: return spriteList[26];
            case Direction.SOUTH_WEST: return spriteList[24];
            case Direction.NORTH_WEST: return spriteList[0];
            default: return null;
        }
    }

    private Sprite CutSprite(int index)
    {
        return Sprite.Create(tileset, new Rect((index % 8) * 24, (7 - index / 8) * 24, 24, 24), new Vector2(0.5f, 0.5f), 24);
    }
}
