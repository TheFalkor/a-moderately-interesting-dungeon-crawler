using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaTooltip : MonoBehaviour
{
    [Header("UI References")]
    private Text headerText;
    [Space]
    private Text leftSubHeaderText;
    private Text rightSubHeaderText;
    [Space]
    private GameObject rightHeaderIcon;
    [Space]
    private Text descriptionText;
    [Space]
    private Transform statsParent;
    private Image stat1Image;
    private Image stat2Image;
    private Image stat3Image;

    [Header("Sprites")]
    private Sprite attackSprite;
    private Sprite healthSprite;
    private Sprite defenseSprite;

    [Header("Runtime Variables")]
    private bool mouseOn = false;
    private float delay = 0;
    private const float DELAY_TIME = 0.1f;
    private RectTransform target;
    private Transform parent;
    private RectTransform body;

    [Header("Singleton")]
    public static MetaTooltip instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        body = GetComponent<RectTransform>();
        parent = transform.parent;

        headerText = transform.GetChild(0).GetComponent<Text>();
        leftSubHeaderText = transform.GetChild(1).GetComponent<Text>();
        rightSubHeaderText = transform.GetChild(2).GetComponent<Text>();
        rightHeaderIcon = rightSubHeaderText.transform.GetChild(0).gameObject;

        descriptionText = transform.GetChild(3).GetComponent<Text>();

        statsParent = transform.GetChild(4);

        stat1Image = statsParent.GetChild(0).GetComponent<Image>();
        stat2Image = statsParent.GetChild(1).GetComponent<Image>();
        stat3Image = statsParent.GetChild(2).GetComponent<Image>();

        attackSprite = stat1Image.sprite;
        healthSprite = stat2Image.sprite;
        defenseSprite = stat3Image.sprite;

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!mouseOn)
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Show(RectTransform target, TooltipData data)
    {
        this.target = target;
        body.SetParent(target);

        int xPosition = (int)target.position.x;

        if (xPosition >= -3)
            body.localPosition = target.position + new Vector3(-200, 0);
        else
            body.localPosition = target.position + new Vector3(200, 0);

        body.SetParent(parent);
        body.localScale = Vector3.one;

        headerText.text = data.header;
        leftSubHeaderText.text = data.leftHeader;
        rightSubHeaderText.text = data.rightHeader;
        rightHeaderIcon.SetActive(data.iconOnRightHeader);

        descriptionText.text = data.description;

        if (data.stat1 != null)
        {
            stat1Image.gameObject.SetActive(true);
            stat1Image.sprite = GetSprite(data.stat1Type);
            stat1Image.transform.GetChild(0).GetComponent<Text>().text = data.stat1;
        }
        else
            statsParent.gameObject.SetActive(false);

        if (data.stat2 != null)
        {
            stat2Image.gameObject.SetActive(true);
            stat2Image.sprite = GetSprite(data.stat2Type);
            stat2Image.transform.GetChild(0).GetComponent<Text>().text = data.stat2;
        }
        else
            stat2Image.gameObject.SetActive(false);

        if (data.stat3 != null)
        {
            stat3Image.gameObject.SetActive(true);
            stat3Image.sprite = GetSprite(data.stat3Type);
            stat3Image.transform.GetChild(0).GetComponent<Text>().text = data.stat3;
        }
        else
            stat3Image.gameObject.SetActive(false);

        mouseOn = true;
        gameObject.SetActive(true);
    }

    public void MouseOnTarget(RectTransform target)
    {
        if (this.target == target)
            mouseOn = true;
    }

    public void MouseOffTarget(RectTransform target)
    {
        if (this.target == target)
        {
            mouseOn = false;
            delay = DELAY_TIME;
        }
    }

    private Sprite GetSprite(string type)
    {
        switch (type)
        {
            case "ATK": return attackSprite;
            case "DEF": return defenseSprite;
            case "HP": return healthSprite;
            default: return null;
        }
    }
}
