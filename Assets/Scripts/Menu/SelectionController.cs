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
    [SerializeField] private Image background;
    [Space]
    [SerializeField] private RectTransform raceParent;
    [SerializeField] private RectTransform classParent;
    [Space]
    [SerializeField] private Text raceText;
    [SerializeField] private Text classText;
    [Space]
    [SerializeField] private Text raceDescription;
    [SerializeField] private Text classDescription;


    [Header("Runtime variables")]
    private int currentRaceIndex = 0;
    private int currentClassIndex = 0;
    private List<RectTransform> raceSelectors = new List<RectTransform>();
    private List<RectTransform> classSelectors = new List<RectTransform>();


    private void Start()
    {
        UpdateUI();

        for (int i = 1; i < raceParent.childCount; i++)
            raceSelectors.Add(raceParent.GetChild(i).GetComponent<RectTransform>());

        for (int i = 1; i < classParent.childCount; i++)
            classSelectors.Add(classParent.GetChild(i).GetComponent<RectTransform>());

        SwitchRace(0);
        SwitchClass(0);
    }

    public void SwitchRace(int index)
    {
        raceSelectors[currentRaceIndex].GetComponentInChildren<Button>().interactable = true;
        raceSelectors[currentRaceIndex].GetChild(1).gameObject.SetActive(false);
        currentRaceIndex = index;
        raceSelectors[currentRaceIndex].GetComponentInChildren<Button>().interactable = false;
        raceSelectors[currentRaceIndex].GetChild(1).gameObject.SetActive(true);
        raceSelectors[currentRaceIndex].transform.SetAsLastSibling();

        gameObject.GetComponent<MainMenuManager>().OnClick();
        UpdateUI();
    }

    public void SwitchClass(int index)
    {
        classSelectors[currentClassIndex].GetComponentInChildren<Button>().interactable = true;
        classSelectors[currentClassIndex].GetChild(1).gameObject.SetActive(false);
        currentClassIndex = index;
        classSelectors[currentClassIndex].GetComponentInChildren<Button>().interactable = false;
        classSelectors[currentClassIndex].GetChild(1).gameObject.SetActive(true);
        classSelectors[currentClassIndex].transform.SetAsLastSibling();

        gameObject.GetComponent<MainMenuManager>().OnClick();
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

        BaseStatsSO selectedRace = raceList[currentRaceIndex];
        ClassStatsSO selectedClass = classList[currentClassIndex];

        background.sprite = selectedClass.backgroundSplash;

        raceText.text = selectedRace.entityName;
        classText.text = selectedClass.className;

        raceDescription.text = selectedRace.entityName + ": " + selectedRace.entityDescription;
        classDescription.text = selectedClass.className + ": " + selectedClass.classDescription;
    }
}
