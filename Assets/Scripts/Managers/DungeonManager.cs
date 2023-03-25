using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;
    [SerializeField] private DungeonLayout nodeLayout;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject dungeonProfile;
    public GameObject miniPlayer;
    public Player player;

    [Header("Runtime Variables")]
    [HideInInspector] public List<DungeonNode> allNodes = new List<DungeonNode>();
    private DungeonNode currentNode;
    [HideInInspector] public bool allowSelection = true;
    private bool start = false;
    private int roomsCompleted = 0;
    private int roomsAmount = -1;
    private AudioCore audioCore;

    [Header("Treasure Dependencies")]
    [SerializeField] private GameObject treasureParent;
    [SerializeField] private Transform treasureRewardTransform;
    private Inventory playerInventory;
    private List<Hoverable> treasureRewardIconList = new List<Hoverable>();

    [Header("Event Dependencies")]
    [SerializeField] private GameObject eventParent;
    [SerializeField] private Text eventTitle;
    [SerializeField] private Text eventDescription;

    // This bool is here to trigger the new overworld code without disrupting the functioning of main
    [Header("Testing")]
    public bool testingOverworld = false;
    

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

        miniPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = player.baseStat.miniSprite;

        foreach (Transform hover in treasureRewardTransform)
            treasureRewardIconList.Add(hover.GetComponent<Hoverable>());
    }

    private void Start()
    {
        dungeonProfile.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = player.baseStat.racePortrait;
        dungeonProfile.transform.GetChild(1).GetComponent<Text>().text = player.baseStat.entityName;
        dungeonProfile.transform.GetChild(2).GetComponent<Text>().text = player.classStat.className;
        audioCore = gameObject.GetComponent<AudioCore>();

        playerInventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        InitializeLayout(); // will be removed once the overworld is complete, needed to make the base game still work
    }

    public void InitializeLayout()
    {
        if (!nodeLayout)
            return;

        nodeLayout.startNode.Initialize(nodeLayout.startNode.room, nodeLayout.startNode);

        miniPlayer.transform.parent = allNodes[0].transform;
        miniPlayer.transform.localPosition = new Vector3(0, 0.5f);
        miniPlayer.transform.localScale = new Vector3(-1, 1, 1);
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

        if (Input.GetKeyUp(KeyCode.F10))
        {
            foreach (DungeonNode node in allNodes)
                node.gameObject.SetActive(true);
        }
    }

    public void LoadDungeon(GameObject layout)
    {
        transitionAnimator.SetBool("Closed", false);

        dungeonParent.SetActive(true);

        ResetGame();

        GameObject NodeLayout = Instantiate(layout, dungeonParent.transform);
        
        if (nodeLayout)
            Destroy(nodeLayout.gameObject);
        nodeLayout = NodeLayout.GetComponent<DungeonLayout>();

        StartCoroutine(DelayInitialization());
    }

    private IEnumerator DelayInitialization()
    {
        yield return new WaitForSeconds(0.1f);
        InitializeLayout();
    }

    public void EnterRoom(DungeonNode node)
    {
        if (!allowSelection)
            return;

        currentNode = node;
        allowSelection = false;

        if (currentNode.room.roomType == RoomType.COMBAT)
            transitionAnimator.SetBool("Closed", true);

        StartCoroutine(DelayRoom());
    }

    public void RemoveRestrictions()
    {
        StartCoroutine(RestrictionsCooldown());
    }

    private IEnumerator RestrictionsCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        allowSelection = true;
    }

    public void ToggleDungeonVisibility(bool active)
    {
        dungeonParent.SetActive(active);

        if (active)
            allowSelection = true;
    }

    private void ChangeMusic(string name)
    {
        audioCore.PlayMusic(name, AudioCore.Track.A);
    }

    public void WonRoom()
    {
        if (AbilityTree.instance.playerLevel < 9)
            AbilityTree.instance.AddSkillPoint(1);

        CompleteRoom();
    }

    private void CompleteRoom()
    {
        miniPlayer.transform.parent = currentNode.transform;
        Vector2 deltaPosition = new Vector2(miniPlayer.transform.position.x - currentNode.transform.position.x, 0);
        miniPlayer.transform.localPosition = new Vector3(0, 0.5f);

        if (deltaPosition.x != 0)
            miniPlayer.transform.localScale = new Vector2(deltaPosition.normalized.x, 1);

        currentNode.MarkCompleted();

        if (currentNode.room.roomType != RoomType.COMBAT)
        {
            allowSelection = true;
            return;
        }

        roomsCompleted++;
        if (roomsCompleted > roomsAmount)
        {
            if (testingOverworld)
            {
                EndManager.instance.SetExitDungeonPopup(true);
            }

            else
            {
                transitionAnimator.SetBool("Closed", true);
                EndManager.instance.EndGame(true);
            }
        }
    }

    public void ExitDungeon()
    {
        transitionAnimator.SetBool("Closed", true);
        EndManager.instance.SetExitDungeonPopup(false);
        StartCoroutine(DelayExit());
    }

    public DungeonNode GetCurrentNode()
    {
        return currentNode;
    }

    public void AddCombatRoom()
    {
        roomsAmount++;
    }

    public void ResetGame()
    {
        roomsAmount = -1;
        roomsCompleted = 0;

        allNodes.Clear();
        miniPlayer.transform.parent = dungeonParent.transform;
    }

    private IEnumerator DelayRoom()
    {
        switch (currentNode.room.roomType)
        {
            case RoomType.COMBAT:
                yield return new WaitForSeconds(1.25f);
                ToggleDungeonVisibility(false);
                TopBarManager.instance.SetVisible(false);
                CombatManager.instance.StartCombat((CombatRoomSO)currentNode.room);
                ChangeMusic("COMBAT");
                break;

            case RoomType.EVENT:
                EventRoom((EventRoomSO)currentNode.room);
                Debug.Log("event dog");
                break;

            case RoomType.TREASURE:
                TreasureRoom((TreasureRoomSO)currentNode.room);
                Debug.Log("treasure dog");
                break;
        }
    }

    private IEnumerator DelayExit()
    {
        yield return new WaitForSeconds(1.25f);

        ToggleDungeonVisibility(false);
        OverworldManager.instance.SetVisible(true);
    }

    private void TreasureRoom(TreasureRoomSO room)
    {
        treasureParent.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            if (currentNode.rewardList.Count <= i)
                treasureRewardIconList[i].gameObject.SetActive(false);
            else
            {
                ItemSO item = currentNode.rewardList[i];

                TooltipData data = Inventory.ItemToData(item, true);

                treasureRewardIconList[i].SetInformation(data);
                treasureRewardIconList[i].transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;

                treasureRewardIconList[i].gameObject.SetActive(true);

                playerInventory.AddItem(Inventory.CreateItem(item));
            }

        }

        for (int i = 0; i < 4; i++)
        {
            if (currentNode.rewardList.Count <= i)
                break;

            if (currentNode.rewardList[i].itemType != ItemType.CONSUMABLE && currentNode.rewardList[i].itemType != ItemType.THROWABLE)
            {
                currentNode.rewardList.RemoveAt(i);
                i--;
            }
        }
    }

    public void TreasurePopUpClosed()
    {
        CompleteRoom();
        treasureParent.SetActive(false);
    }

    private void EventRoom(EventRoomSO room)
    {
        eventTitle.text = room.EventTitle;
        eventDescription.text = room.EventDescription;

        eventParent.SetActive(true);
    }

    public void EventPopUpClosed()
    {
        CompleteRoom();
        eventParent.SetActive(false);
    }
}
