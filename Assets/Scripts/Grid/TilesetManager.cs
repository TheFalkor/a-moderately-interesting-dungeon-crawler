using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesetManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D tileset;
    public Sprite[] borderBricks = new Sprite[8];
    private Sprite[] walls = new Sprite[5];
    private Sprite[] blackBox = new Sprite[8];
    private Sprite[] sideFloor = new Sprite[7];
    private Sprite[] randomFloor = new Sprite[4];

    private List<int> wallIndeces = new List<int>();
    private List<int> groundIndeces = new List<int>();

    [Header("Singleton")]
    public static TilesetManager instance;


    void Awake()
    {
        if (instance)
            return;

        instance = this;

        borderBricks[0] = CutSprite(0);
        borderBricks[1] = CutSprite(23);
        borderBricks[2] = CutSprite(2);
        borderBricks[3] = CutSprite(8);
        borderBricks[4] = CutSprite(10);
        borderBricks[5] = CutSprite(12);
        borderBricks[6] = CutSprite(13);
        borderBricks[7] = CutSprite(14);

        walls[0] = CutSprite(28);
        walls[1] = CutSprite(1);
        walls[2] = CutSprite(30);
        walls[3] = CutSprite(31);
        walls[4] = CutSprite(5);

        blackBox[0] = CutSprite(24);
        blackBox[1] = CutSprite(25);
        blackBox[2] = CutSprite(26);
        blackBox[3] = CutSprite(27);
        blackBox[4] = CutSprite(29);
        blackBox[5] = CutSprite(18);
        blackBox[6] = CutSprite(19);
        blackBox[7] = CutSprite(22);

        sideFloor[0] = CutSprite(16);
        sideFloor[1] = CutSprite(9);
        sideFloor[2] = CutSprite(17);
        sideFloor[3] = CutSprite(24);
        sideFloor[4] = CutSprite(25);
        sideFloor[5] = CutSprite(4);
        sideFloor[6] = CutSprite(6);

        randomFloor[0] = CutSprite(3);
        randomFloor[1] = CutSprite(7);
        randomFloor[2] = CutSprite(11);
        randomFloor[3] = CutSprite(15);

        wallIndeces.Add(1);
        wallIndeces.Add(5);
        wallIndeces.Add(18);
        wallIndeces.Add(19);
        wallIndeces.Add(22);
        wallIndeces.Add(24);
        wallIndeces.Add(25);
        wallIndeces.Add(26);
        wallIndeces.Add(27);
        wallIndeces.Add(28);
        wallIndeces.Add(29);
        wallIndeces.Add(30);
        wallIndeces.Add(31);

        groundIndeces.Add(3);
        groundIndeces.Add(7);
        groundIndeces.Add(11);
        groundIndeces.Add(15);
        groundIndeces.Add(4);
        groundIndeces.Add(6);
        groundIndeces.Add(9);
        groundIndeces.Add(16);
        groundIndeces.Add(17);
        groundIndeces.Add(20);
        groundIndeces.Add(21);
    }

    public void CalculateWallTile(Tile currentTile)
    {
        List<int> availableSprites = new List<int>();
        availableSprites.AddRange(wallIndeces);

        Tile upTile = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down);
        if (upTile && upTile.IsWalkable())    // No wall above
        {
            availableSprites.Remove(1);
            availableSprites.Remove(5);
            availableSprites.Remove(18);
            availableSprites.Remove(19);
            availableSprites.Remove(22);
            availableSprites.Remove(28);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }
        else
        {
            availableSprites.Remove(18);
            availableSprites.Remove(19);
            availableSprites.Remove(24);
            availableSprites.Remove(25);
            availableSprites.Remove(26);
            availableSprites.Remove(29);
        }

        Tile downTile = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up);
        if (downTile && downTile.IsWalkable())      // No wall below
        {
            availableSprites.Remove(18);
            availableSprites.Remove(19);
            availableSprites.Remove(22);
            availableSprites.Remove(24);
            availableSprites.Remove(25);
            availableSprites.Remove(26);
            availableSprites.Remove(27);
            availableSprites.Remove(29);

            availableSprites.Remove(1);
            availableSprites.Remove(28);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }
        else
        {
            availableSprites.Remove(5);
        }

        Tile leftTile = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left);
        if (leftTile && leftTile.IsWalkable())
        {
            availableSprites.Remove(1);
            availableSprites.Remove(19);
            availableSprites.Remove(22);
            availableSprites.Remove(25);
            availableSprites.Remove(26);
            availableSprites.Remove(30);
        }
        else
        {
            availableSprites.Remove(18);
            availableSprites.Remove(24);
            availableSprites.Remove(27);
            availableSprites.Remove(28);
            availableSprites.Remove(29);
            availableSprites.Remove(31);
        }

        Tile rightTile = GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right);
        if (rightTile && rightTile.IsWalkable())
        {
            availableSprites.Remove(18);
            availableSprites.Remove(22);
            availableSprites.Remove(24);
            availableSprites.Remove(25);
            availableSprites.Remove(28);
        }
        else
        {
            availableSprites.Remove(19);
            availableSprites.Remove(26);
            availableSprites.Remove(27);
            availableSprites.Remove(29);
            availableSprites.Remove(30);
            availableSprites.Remove(31);
        }

        availableSprites.Remove(22);
        Debug.Log(currentTile.GetPosition() + " :: " + availableSprites[0]);
        //return borderBricks[0];
        currentTile.GetComponent<SpriteRenderer>().sprite = CutSprite(availableSprites[0]);
       // currentTile.transform.GetComponent<SpriteRenderer>().sprite = borderBricks[0];
    }

    private Sprite CutSprite(int index)
    {
        return Sprite.Create(tileset, new Rect((index % 4) * 24, (7 - index / 4) * 24, 24, 24), new Vector2(0.5f, 0.5f), 24);
    }
}
