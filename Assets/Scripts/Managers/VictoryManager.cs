using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject victoryPopup;

    private float timer = 0;
    private bool count = false;

    [Header("Victory Info")]
    [SerializeField] private GameObject textBox;
    private Text lvlText;
    private Text xpText;
    private Text moneyText;
    [SerializeField] private Image playerPortrait;

    [Header("Static Names")]
    private string className;
    private string raceName;

    [Header("Runtime Variables")]
    private int level = 0;

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
    }

    private void Start()
    {
        player = DungeonManager.instance.player;
        raceName = player.baseStat.entityName;
        className = player.classStat.className;
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
        level++;
        UpdateVictory();
        victoryPopup.SetActive(true);
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
        lvlText.text = "Lv." + level + " " + raceName + ", " + className;
        //xpText.text = "XP: " + player.currentExp.ToString() + "/" + player.levelExp.ToString() + " (" + expGain.ToString() + ")";
        moneyText.text = "You have gained an Ability Point!";
    }

    public void SetPortrait(Sprite sprite)
    {
        playerPortrait.sprite = sprite;
    }
}
