using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;
    [SerializeField] private Animator transitionAnimator;
    public Player player;

    [Header("Runtime Variables")]
    private DungeonNode currentNode;
    private bool roomSelected = false;
    private float transitionTimer = 0;
    private bool allowSelection = true;
    private bool start = false;
    private int roomsCompleted = 0;
    private int roomsAmount = -1;
    

    [Header("Singleton")]
    public static DungeonManager instance;

    void Awake()
    {
        if (instance)
            return;

        instance = this;

        if (ConsistentData.initialized)
            player.Setup(ConsistentData.playerBaseStat, ConsistentData.playerClassStat);
        else
            player.Setup();
    }

    void Update()
    {
        if (!start)
        {
            ChangeMusic("DUNGEON");
            start = true;
        }

        if (!allowSelection)
            return;

        if (!roomSelected && Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.transform.GetComponent<DungeonNode>())
                {
                    if (!hit.transform.GetComponent<DungeonNode>().completed)
                    {
                        currentNode = hit.transform.GetComponent<DungeonNode>();
                        roomSelected = true;
                        transitionAnimator.SetBool("Closed", true);
                    }

                }
            }
        }

        if (roomSelected)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer > 1.25f)
            {
                transitionTimer = 0;
                roomSelected = false;

                currentNode.EnterNode();
                ChangeMusic("COMBAT_2");
            }
        }
    }

    public void OpenInventory()
    {
        allowSelection = false;
        InventoryUI.instance.ShowUI();
    }

    public void RemoveRestrictions()
    {
        allowSelection = true;
    }

    public void ToggleDungeonVisibility(bool active)
    {
        dungeonParent.SetActive(active);

    }

    private void ChangeMusic(string name)
    {
        gameObject.GetComponent<AudioKor>().PlayMusic(name, AudioKor.Track.A);
    }

    public void WonRoom()
    {
        AbilityTree.instance.skillPoints++;

        currentNode.MarkCompleted();
        ChangeMusic("DUNGEON");

        //Check Win Condition
        roomsCompleted++;
        if(roomsCompleted >= roomsAmount)
        {
            transitionAnimator.SetBool("Closed", true);
            EndManager.instance.EndGame(true);
        }
    }

    public void AddRoom()
    {
        roomsAmount++;
    }
}
