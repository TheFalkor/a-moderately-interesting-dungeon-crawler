using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaTooltip : MonoBehaviour
{
    [Header("UI References")]
    private Text headerText;
    private Image headerIcon;
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
    private List<RectTransform> metaPoints = new List<RectTransform>();

    [Header("Singleton")]
    public static MetaTooltip instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        body = GetComponent<RectTransform>();
        parent = transform.parent;

        for (int i = 2; i < parent.childCount; i++)
            metaPoints.Add(parent.GetChild(i).GetComponent<RectTransform>());

        headerIcon = transform.GetChild(5).GetComponent<Image>();
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

        body.localPosition = target.position;

        RectTransform rect = metaPoints[0];
        body.SetParent(parent);

        foreach (RectTransform tr in metaPoints)
        {
            float pointMagnitude = (body.transform.position - tr.position).magnitude;
            float currentMagnitude = (body.transform.position - rect.position).magnitude;

            if (pointMagnitude < currentMagnitude && pointMagnitude > 2.4f)
                rect = tr;
        }

        body.SetParent(rect);
        body.anchoredPosition = new Vector3(0, 0, 0);
        body.localScale = Vector3.one;

        headerIcon.gameObject.SetActive(data.headerIcon != null);
        headerIcon.sprite = data.headerIcon;

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
            stat1Image.gameObject.SetActive(false);

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
