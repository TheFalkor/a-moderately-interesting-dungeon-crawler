using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    //  [1]  Click on highlighted tiles to interact, move and attack
    //  [2]  Moving, attacking and using abilities requires action-points
    //  [3]  Press [End Turn] to let the enemies take their turn
    //  [4]  Items don't need action-points and are used by clicking on a highlighted tile
    //  [5]  After every battle you receive an ability point to spend unlocking new abilities
    //  [6]  In the dungeon you can equip different weapons and armor that fits your playstyle

    [Header("Sprite Reference")]
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    [Header("UI References")]
    [SerializeField] private Image screenshotImage;
    [SerializeField] private Text descriptionText;
    [SerializeField] private RectTransform shortcutParent;
    private List<Image> shortcutImageList = new List<Image>();

    [Header("Tutorial Information")]
    [SerializeField] private List<Sprite> screenshotList = new List<Sprite>();
    [SerializeField] private List<string> messageList = new List<string>();

    [Header("Runtime Variables")]
    private int currentIndex = 0;

    void Start()
    {
        for (int i = 0; i < shortcutParent.childCount; i++)
            shortcutImageList.Add(shortcutParent.GetChild(i).GetComponent<Image>());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
            SelectTutorial(currentIndex - 1);
        else if (Input.GetKeyUp(KeyCode.D))
            SelectTutorial(currentIndex + 1);
            
    }

    public void SelectTutorial(int index)
    {
        if (index < 0)
            index = shortcutImageList.Count - 1;

        index %= shortcutImageList.Count;

        screenshotImage.sprite = screenshotList[index];
        descriptionText.text = messageList[index];

        shortcutImageList[currentIndex].sprite = offSprite;
        shortcutImageList[index].sprite = onSprite;

        currentIndex = index;
    }

    public void ChangeTutorial(int direction)
    {
        SelectTutorial(currentIndex + direction);
    }
}
