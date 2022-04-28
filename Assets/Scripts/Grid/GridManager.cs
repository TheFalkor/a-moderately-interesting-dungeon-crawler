using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject tilePrefab;

    [Header("Grid Settings")]
    private const int RANDOM_WALL_CHANCE = 10;
    private const int ROOM_WIDTH = 10;
    private const int ROOM_HEIGHT = 7;

    [Header("Runtime Variables")]
    private readonly List<Tile> tileList = new List<Tile>();
    private readonly List<SpriteRenderer> backdropRenders= new List<SpriteRenderer>();

    [Header("Singleton")]
    public static GridManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void CreateRoom()
    {
        GameObject parent = new GameObject("Room");
        parent.transform.parent = GameObject.Find("Combat Parent").transform;
        for (int i = 0; i < ROOM_WIDTH * ROOM_HEIGHT; i++)
        {
            Tile temp = Instantiate(tilePrefab, new Vector2(-ROOM_WIDTH / 2.0f + i % ROOM_WIDTH + 0.5f, ROOM_HEIGHT / 2.0f - i / ROOM_WIDTH), Quaternion.identity, parent.transform).GetComponentInParent<Tile>();
            temp.Initialize(new Vector2Int(i % 10, i / 10));

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

    private void CreateBorder()
    {
        GameObject parent = new GameObject("Room Border");
        parent.transform.parent = GameObject.Find("Combat Parent").transform;

        for (int i = 0; i < ROOM_HEIGHT + 3; i++)
        {
            GameObject border = new GameObject("Left Border");
            border.transform.parent = parent.transform;
            border.transform.position = new Vector3(-ROOM_WIDTH / 2 - 0.5f, ROOM_HEIGHT / 2 - i + 2.5f);

            SpriteRenderer render = border.AddComponent<SpriteRenderer>();
            if (i == 0)
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.NORTH_WEST);
            else if (i == ROOM_HEIGHT + 2)
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.SOUTH_WEST);
            else
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.WEST);
            render.sortingOrder = -10;
        }

        for (int i = 0; i < ROOM_HEIGHT + 3; i++)
        {
            GameObject border = new GameObject("Right Border");
            border.transform.parent = parent.transform;
            border.transform.position = new Vector3(ROOM_WIDTH / 2 + 0.5f, ROOM_HEIGHT / 2 - i + 2.5f);

            SpriteRenderer render = border.AddComponent<SpriteRenderer>();
            if (i == 0)
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.NORTH_EAST);
            else if (i == ROOM_HEIGHT + 2)
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.SOUTH_EAST);
            else
                render.sprite = TilesetManager.instance.GetBorderSprite(Direction.EAST);
            render.sortingOrder = -10;
        }

        for (int i = 0; i < ROOM_WIDTH; i++)
        {
            GameObject border = new GameObject("Bottom Border");
            border.transform.parent = parent.transform;
            border.transform.position = new Vector3(-ROOM_WIDTH / 2 + 0.5f + i, -ROOM_HEIGHT / 2 - 0.5f);

            SpriteRenderer render = border.AddComponent<SpriteRenderer>();
            render.sprite = TilesetManager.instance.GetBorderSprite(Direction.SOUTH);
            render.sortingOrder = -10;
        }

        for (int i = 0; i < ROOM_WIDTH * 2; i++)
        {
            GameObject border = new GameObject("Backdrop");
            border.transform.parent = parent.transform;
            border.transform.position = new Vector3(-ROOM_WIDTH / 2 + 0.5f + i / 2, ROOM_HEIGHT / 2 + 2.5f - i % 2);

            SpriteRenderer render = border.AddComponent<SpriteRenderer>();
            render.sortingOrder = -10;

            backdropRenders.Add(render);
        }
    }

    public void GenerateCombat(CombatRoomSO room)
    {
        if (tileList.Count == 0)
        {
            CreateBorder();
            CreateRoom();
        }

        // Destroy all existing occupants

        for (int i = 0; i < room.tiles.Length; i++)
        {
            tileList[i].Setup(room.tiles[i].wall);

            if (!room.tiles[i].wall && room.tiles[i].occupantPrefab)
            {
                GameObject temp = Instantiate(room.tiles[i].occupantPrefab, tileList[i].transform.position, Quaternion.identity);

                if (temp.GetComponent<Entity>())
                {
                    CombatManager.instance.AddEntity(temp.GetComponent<Entity>());
                    temp.GetComponent<Entity>().Initialize();
                }
                else
                    CombatManager.instance.AddOccupant(temp.GetComponent<Occupant>());
            }
        }

        foreach (Tile tile in tileList)
        {
            tile.UpdateTileset();
        }

        return;
        
        // WIP
        for (int i = 0; i < ROOM_WIDTH; i++)
        {
            Tile tile = GetTile(i);
            bool wall0 = !tile || !tile.IsWalkable();

            tile = GetTile(i + ROOM_WIDTH);
            bool wall1 = !tile || !tile.IsWalkable();

            tile = GetTile(i - 1);
            bool wallLeft = !tile || !tile.IsWalkable();

            tile = GetTile(i - 1);
            bool wallRight = !tile || !tile.IsWalkable();

            Sprite topRow = null;
            Sprite bottomRow = null;

            if (wall0 && !wall1)
            {
                if (wallLeft && wallRight)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(1);
                else if (wallLeft)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(30);
                else if (wallRight)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(28);
                else
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(31);
            }
            else if (wall0 && wall1)
            {
                if (wallLeft && wallRight)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(25);
                else if (wallLeft)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(26);
                else if (wallRight)
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(24);
                else
                    backdropRenders[i].sprite = TilesetManager.instance.GetTileSprite(27);
            }
            else
            {

            }

        }
    }

    public void ClearAllHighlights()
    {
        foreach (Tile tile in tileList)
            tile.ClearHighlight();
    }

    public Tile GetTile(int index)
    {
        if (index < 0 || index >= tileList.Count)
            return null;

        return tileList[index];
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
        if (position.x < -ROOM_WIDTH / 2.0f || position.x > ROOM_WIDTH / 2.0f || position.y <= -ROOM_HEIGHT / 2.0f || position.y > ROOM_HEIGHT / 2.0f)
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