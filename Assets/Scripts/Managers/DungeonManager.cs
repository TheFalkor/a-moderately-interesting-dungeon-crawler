using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject exitParent;
    [SerializeField] private GameObject dungeonProfile;
    public GameObject miniPlayer;
    public Player player;

    [Header("Runtime Variables")]
    [HideInInspector] public List<DungeonNode> allNodes = new List<DungeonNode>();
    private DungeonNode currentNode;
    private DungeonNode hoveringNode;
    private bool roomSelected = false;
    private float transitionTimer = 0;
    private bool allowSelection = true;
    private bool start = false;
    private int roomsCompleted = 0;
    private int roomsAmount = -1;
    private AudioKor audioKor;
    

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
        audioKor = gameObject.GetComponent<AudioKor>();
    }

    void Update()
    {
        if (!start)
        {
            ChangeMusic("DUNGEON");
            start = true;
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

        if (!allowSelection)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit && hit.transform.GetComponent<DungeonNode>())
        {
            hoveringNode = hit.transform.GetComponent<DungeonNode>();
            hoveringNode.HighlightNode(true);
        }
        else if (hoveringNode)
        {
            hoveringNode.HighlightNode(false);
            hoveringNode = null;
        }


        if (!roomSelected && Input.GetMouseButtonUp(0))
        {
            if (hoveringNode && !hoveringNode.completed)
            {
                currentNode = hit.transform.GetComponent<DungeonNode>();
                roomSelected = true;
                allowSelection = false;
                transitionAnimator.SetBool("Closed", true);
            }
        }

    }

    public void OpenInventory(int tab)
    {
        allowSelection = false;
        InventoryUI.instance.ShowUI(tab);
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
        audioKor.PlayMusic(name, AudioKor.Track.A);
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

        //Check Win Condition
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

    public void OpenExitPopup()
    {
        allowSelection = false;
        audioKor.PlaySFX("SELECT");
        exitParent.SetActive(true);
    }

    public void ExitPopupChoice(bool endGame)
    {
        if (endGame)
            SceneManager.LoadScene(0);
        else
            exitParent.SetActive(false);

        RemoveRestrictions();
    }
}
