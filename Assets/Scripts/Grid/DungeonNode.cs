using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode : MonoBehaviour
{
    [Header("Dungeon Setup")]
    public CombatRoomSO room;
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


    [Header("Runtime Variables")]
    [HideInInspector] public bool completed = true;
    private SpriteRenderer northRender;
    private SpriteRenderer eastRender;
    private SpriteRenderer southRender;
    private SpriteRenderer westRender;


    private void Awake()
    {
        List<Vector2> directionList = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
        
        foreach (Vector3 dir in directionList)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position + dir / 1.5f, transform.position + dir * 2);

            if (hit)
            {
                if (hit.transform.GetComponent<DungeonNode>())
                    connectedNodes.Add(hit.transform.GetComponent<DungeonNode>());
            }
        }
    }

    void Start()
    {
        foreach (DungeonNode node in connectedNodes)
        {
            Vector2 direction = node.transform.position - transform.position;
            direction.Normalize();

            GameObject temp = new GameObject("Bridge");
            temp.transform.parent = transform;
            temp.transform.localScale = new Vector3(1, 1, 1);
            SpriteRenderer render = temp.AddComponent<SpriteRenderer>();
            render.sortingOrder = 2;

            if (direction.y == 1)
            {
                render.transform.localPosition = new Vector3(0, 0.5f);
                render.sprite = northBrokenBridge;
                northRender = render;
            }
            else if (direction.x == 1)
            {
                render.transform.localPosition = new Vector3(0.5f, 0);
                render.sprite = eastBrokenBridge;
                eastRender = render;
            }
            else if (direction.y == -1)
            {
                render.transform.localPosition = new Vector3(0, -0.5f);
                render.sprite = southBrokenBridge;
                southRender = render;
            }
            else if (direction.x == -1)
            {
                render.transform.localPosition = new Vector3(-0.5f, 0);
                render.sprite = westBrokenBridge;
                westRender = render;
            }
        }


        if (room)
        {
            completed = false;
        }
        else
        {
            Initialize(room, this);
            MarkCompleted();
        }
    }

    public void Initialize(bool locked, DungeonNode parent)
    {
        foreach (DungeonNode node in connectedNodes)
        {
            if (parent != node)
                node.Initialize(room, this);
        }
        DungeonManager.instance.AddRoom();
        gameObject.SetActive(!locked);
    }

    public bool EnterNode()
    {
        if (completed)
            return false;

        CombatManager.instance.StartCombat(room);
        DungeonManager.instance.ToggleDungeonVisibility(false);

        return true;
        //MarkCompleted();    // tmp
    }

    public void MarkCompleted()
    {
        completed = true;
        transform.GetChild(0).gameObject.SetActive(false);

        if (northRender)
        {
            northRender.sprite = verticalBridge;
            northRender.transform.localPosition = new Vector3(0, 1);
            northRender.sortingOrder = 3;
        }
        if (eastRender)
        {
            eastRender.sprite = horizontalBridge;
            eastRender.transform.localPosition = new Vector3(1, 0);
            eastRender.sortingOrder = 3;
        }
        if (southRender)
        {
            southRender.sprite = verticalBridge;
            southRender.transform.localPosition = new Vector3(0, -1);
            southRender.sortingOrder = 3;
        }
        if (westRender)
        {
            westRender.sprite = horizontalBridge;
            westRender.transform.localPosition = new Vector3(-1, 0);
            westRender.sortingOrder = 3;
        }

        foreach (DungeonNode node in connectedNodes)
            node.gameObject.SetActive(true);
    }
}
