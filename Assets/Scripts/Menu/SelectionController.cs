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
    [SerializeField] private Text raceText;
    [SerializeField] private Text classText;


    [Header("Runtime variables")]
    private int currentRaceIndex = 0;
    private int currentClassIndex = 0;


    private void Start()
    {
        UpdateUI();
    }

    public void SwitchRace(int direction)
    {
        currentRaceIndex += direction;
        currentRaceIndex %= raceList.Count;

        if (currentRaceIndex < 0)
            currentRaceIndex = raceList.Count - 1;
        UpdateUI();
    }

    public void SwitchClass(int direction)
    {
        currentClassIndex += direction;
        currentClassIndex %= classList.Count;

        if (currentClassIndex < 0)
            currentClassIndex = classList.Count - 1;

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
