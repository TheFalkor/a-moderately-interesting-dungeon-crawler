using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    [Header("Available selections")]
    public List<BaseStatsSO> raceList = new List<BaseStatsSO>();
    public List<ClassStatsSO> classList = new List<ClassStatsSO>();


    [Header("UI References")]
    [SerializeField] private Image playerImage;
    [Space]
    [SerializeField] private RectTransform raceParent;
    [SerializeField] private RectTransform classParent;
    [Space]
    [SerializeField] private Text raceText;
    [SerializeField] private Text classText;


    [Header("Runtime variables")]
    private int currentRaceIndex = 0;
    private int currentClassIndex = 0;
    private List<Button> raceButtons = new List<Button>();
    private List<Button> classButtons = new List<Button>();


    private void Start()
    {
        UpdateUI();

        for (int i = 1; i < raceParent.childCount; i++)
            raceButtons.Add(raceParent.GetChild(i).GetComponent<Button>());

        for (int i = 1; i < classParent.childCount; i++)
            classButtons.Add(classParent.GetChild(i).GetComponent<Button>());

        SwitchRace(0);
        SwitchClass(0);
    }

    public void SwitchRace(int index)
    {
        raceButtons[currentRaceIndex].interactable = true;
        currentRaceIndex = index;
        raceButtons[currentRaceIndex].interactable = false;

        UpdateUI();
    }

    public void SwitchClass(int index)
    {
        classButtons[currentClassIndex].interactable = true;
        currentClassIndex = index;
        classButtons[currentClassIndex].interactable = false;

        UpdateUI();
    }

    public BaseStatsSO GetBaseStat()
    {
        return raceList[currentRaceIndex];
    }

    public ClassStatsSO GetClassStat()
    {
        return classList[currentClassIndex];
    }

    private void UpdateUI()
    {
        playerImage.sprite = raceList[currentRaceIndex].entitySprite;

        raceText.text = raceList[currentRaceIndex].entityName;
        classText.text = classList[currentClassIndex].className;
    }
}
