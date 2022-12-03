using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;
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
    }

    private void Start()
    {
        dungeonProfile.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = player.baseStat.racePortrait;
        dungeonProfile.transform.GetChild(1).GetComponent<Text>().text = player.baseStat.entityName;
        dungeonProfile.transform.GetChild(2).GetComponent<Text>().text = player.classStat.className;
        audioCore = gameObject.GetComponent<AudioCore>();
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

    public void EnterRoom(DungeonNode node)
    {
        if (!allowSelection)
            return;

        currentNode = node;
        allowSelection = false;

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
            StartCoroutine(RestrictionsCooldown());
    }

    private void ChangeMusic(string name)
    {
        audioCore.PlayMusic(name, AudioCore.Track.A);
    }

    public void WonRoom()
    {
        if (AbilityTree.instance.playerLevel < 9)
            AbilityTree.instance.AddSkillPoint(1);

        miniPlayer.transform.parent = currentNode.transform;
        Vector2 deltaPosition = new Vector2(miniPlayer.transform.position.x - currentNode.transform.position.x, 0);
        miniPlayer.transform.localPosition = new Vector3(0, 0.5f);
        if (deltaPosition.x != 0)
            miniPlayer.transform.localScale = new Vector2(deltaPosition.normalized.x, 1);

        currentNode.MarkCompleted();

        roomsCompleted++;
        if(roomsCompleted >= roomsAmount)
        {
            transitionAnimator.SetBool("Closed", true);
            EndManager.instance.EndGame(true);
        }
    }

    public DungeonNode GetCurrentNode()
    {
        return currentNode;
    }

    public void AddRoom()
    {
        roomsAmount++;
    }

    public void ResetGame()
    {
        roomsAmount = -1;
        roomsCompleted = 0;
        miniPlayer.transform.parent = allNodes[0].transform;
        miniPlayer.transform.localPosition = new Vector3(0, 0.5f);
        miniPlayer.transform.localScale = new Vector3(-1, 1, 1);
    }

    private IEnumerator DelayRoom()
    {
        yield return new WaitForSeconds(1.25f);

        ToggleDungeonVisibility(false);
        TopBarManager.instance.SetVisible(false);

        switch (currentNode.room.roomType)
        {
            case RoomType.COMBAT:
                CombatManager.instance.StartCombat((CombatRoomSO)currentNode.room);
                ChangeMusic("COMBAT");
                break;

            case RoomType.EVENT:
                Debug.Log("event dog");
                break;

            case RoomType.TREASURE:
                Debug.Log("treasure dog");
                break;
        }
    }
}
