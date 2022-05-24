using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject victoryPopup;
    [SerializeField] private Transform rewardParent;
    private Animator victoryAnimator;

    private float timer = 0;
    private bool count = false;

    [Header("Victory Info")]
    [SerializeField] private GameObject textBox;
    private Text lvlText;
    private Text xpText;
    private Text moneyText;
    [SerializeField] private Image playerPortrait;
    private List<Hoverable> rewardIconList = new List<Hoverable>();

    [Header("GameObject References")]
    private Player player;
    private Inventory inventory;

    [Header("Singleton")]
    public static VictoryManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        lvlText = textBox.transform.Find("Level & Race Text").GetComponent<Text>();
        xpText = textBox.transform.Find("XP Text").GetComponent<Text>();
        moneyText = textBox.transform.Find("Money Text").GetComponent<Text>();

        victoryAnimator = victoryPopup.transform.GetChild(0).GetChild(1).GetComponent<Animator>();

        foreach (Transform hover in rewardParent)
            rewardIconList.Add(hover.GetComponent<Hoverable>());
    }

    private void Start()
    {
        player = DungeonManager.instance.player;
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        SetPortrait(player.baseStat.entitySprite);
    }

    void Update()
    {
        if (count)
        {
            timer += Time.deltaTime;

            if (timer > 1.25f)
            {
                count = false;
                victoryPopup.SetActive(false);
                DungeonManager.instance.ToggleDungeonVisibility(true);
                CombatManager.instance.HideRoom();
                transitionAnimator.SetBool("Closed", false);
            }
        }
    }

    public void ShowPopup()
    {
        AbilityTree.instance.playerLevel++;
        UpdateVictory();
        StartCoroutine(DelayPopup());
    }

    private IEnumerator DelayPopup()
    {
        yield return new WaitForSeconds(0.5f);
        victoryPopup.SetActive(true);
        victoryAnimator.Play("Victory Popup");
    }

    public void ReturnDungeon()
    {
        if (count)
            return;

        count = true;
        timer = 0;
        DungeonManager.instance.WonRoom();
        transitionAnimator.SetBool("Closed", true);
    }

    public void UpdateVictory()
    {
        lvlText.text = "Lv." + AbilityTree.instance.playerLevel + " " + player.baseStat.entityName + " " + player.classStat.className;
        //xpText.text = "XP: " + player.currentExp.ToString() + "/" + player.levelExp.ToString() + " (" + expGain.ToString() + ")";
        xpText.text = "";
        if (AbilityTree.instance.playerLevel < 9)
            moneyText.text = "You have gained an Ability Point!";
        else
            moneyText.text = "";

        DungeonNode node = DungeonManager.instance.GetCurrentNode();

        for (int i = 0; i < 4; i++)
        {
            if (node.rewardList.Count <= i)
                rewardIconList[i].gameObject.SetActive(false);
            else
            {
                ItemSO item = node.rewardList[i];

                TooltipData data = Inventory.ItemToData(item, false);

                rewardIconList[i].SetInformation(data);
                rewardIconList[i].transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;

                rewardIconList[i].gameObject.SetActive(true);

                inventory.AddItem(inventory.CreateItem(item));

            }

        }

        for (int i = 0; i < 4; i++)
        {
            if (node.rewardList.Count <= i)
                break;

            if (node.rewardList[i].itemType != ItemType.CONSUMABLE && node.rewardList[i].itemType != ItemType.THROWABLE)
            {
                node.rewardList.RemoveAt(i);
                i--;
            }
        }

        HotbarUI.instance.UpdateUI();
    }

    public void SetPortrait(Sprite sprite)
    {
        playerPortrait.sprite = sprite;
    }
}
