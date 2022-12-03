using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode : MonoBehaviour
{
    [Header("Dungeon Setup")]
    public BaseRoomSO room;
    [Space]
    private List<DungeonNode> connectedNodes = new List<DungeonNode>();
    
    [Header("Bridge Sprites")]
    public Sprite northBrokenBridge;
    public Sprite eastBrokenBridge;
    public Sprite southBrokenBridge;
    public Sprite westBrokenBridge;
    [Space]
    public Sprite horizontalBridge;
    public Sprite verticalBridge;

    [Header("Hovering Icons")]
    public Sprite[] icons;

    [Header("Runtime Variables")]
    [HideInInspector] public List<ItemSO> rewardList = new List<ItemSO>();
    [HideInInspector] public bool completed = true;
    private DungeonNode parentNode = null;
    private SpriteRenderer render;


    private void Awake()
    {
        List<Vector2> directionList = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        
        foreach (Vector3 dir in directionList)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position + dir / 1.2f, transform.position + dir * 2);

            if (hit)
            {
                if (hit.transform.GetComponent<DungeonNode>())
                    connectedNodes.Add(hit.transform.GetComponent<DungeonNode>());
            }
        }
    }

    void Start()
    {
        if (room)
        {
            foreach (ItemSO item in room.rewards)
                rewardList.Add(item);

            completed = false;

            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = icons[(int)room.roomType];
        }
        else
        {
            Initialize(room, this);
            MarkCompleted();
        }
    }

    public void Initialize(bool locked, DungeonNode parent)
    {
        DungeonManager.instance.allNodes.Add(this);

        parentNode = parent;

        foreach (DungeonNode node in connectedNodes)
        {
            if (parent != node)
                node.Initialize(room, this);
        }
        DungeonManager.instance.AddRoom();
        gameObject.SetActive(!locked);

        if (!render)
            SetupSprites();
    }

    public void MarkCompleted()
    {
        completed = true;
        transform.GetChild(0).gameObject.SetActive(false);

        HighlightNode(false);

        foreach (DungeonNode node in connectedNodes)
            node.gameObject.SetActive(true);
    }

    private void HighlightNode(bool active)
    {
        if (active && !completed)
            render.color = new Color(0.85f, 0.85f, 0.85f);
        else
            render.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (completed)
            return;

        DungeonManager.instance.EnterRoom(this);
    }

    private void OnMouseEnter()
    {
        if (completed)
            return;

        HighlightNode(true);
    }

    private void OnMouseExit()
    {
        if (completed)
            return;

        HighlightNode(false);
    }

    public void GGC_NewGamePlus()
    {
        if (room)
        {
            completed = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void SetupSprites()
    {
        render = GetComponent<SpriteRenderer>();

        foreach (DungeonNode node in connectedNodes)
        {
            Vector2 direction = node.transform.position - transform.position;
            direction.Normalize();

            if (node != parentNode)
            {
                GameObject temp = new GameObject("Bridge");
                temp.transform.parent = transform;
                temp.transform.localScale = new Vector3(1, 1, 1);
                SpriteRenderer render = temp.AddComponent<SpriteRenderer>();
                render.sortingOrder = 2;

                if (direction.y == 1)
                {
                    render.transform.localPosition = new Vector3(0, 0.5f);
                    render.sprite = northBrokenBridge;
                }
                else if (direction.x == 1)
                {
                    render.transform.localPosition = new Vector3(0.5f, 0);
                    render.sprite = eastBrokenBridge;
                }
                else if (direction.y == -1)
                {
                    render.transform.localPosition = new Vector3(0, -0.5f);
                    render.sprite = southBrokenBridge;
                }
                else if (direction.x == -1)
                {
                    render.transform.localPosition = new Vector3(-0.5f, 0);
                    render.sprite = westBrokenBridge;
                }
            }
            else
            {
                GameObject parentBridge = new GameObject("Parent Bridge");
                parentBridge.transform.parent = transform;
                parentBridge.transform.localPosition = direction;
                SpriteRenderer parentRender = parentBridge.AddComponent<SpriteRenderer>();

                if (direction.x == 0)
                    parentRender.sprite = verticalBridge;
                else
                    parentRender.sprite = horizontalBridge;

                parentRender.sortingOrder = 3;
            }
        }
    }
}
