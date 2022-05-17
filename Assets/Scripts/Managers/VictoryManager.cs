using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject victoryPopup;
    private Animator victoryAnimator;

    private float timer = 0;
    private bool count = false;

    [Header("Victory Info")]
    [SerializeField] private GameObject textBox;
    private Text lvlText;
    private Text xpText;
    private Text moneyText;
    [SerializeField] private Image playerPortrait;

    [Header("GameObject References")]
    private Player player;

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
    }

    private void Start()
    {
        player = DungeonManager.instance.player;
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
        moneyText.text = "You have gained an Ability Point!";
    }

    public void SetPortrait(Sprite sprite)
    {
        playerPortrait.sprite = sprite;
    }
}
