using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiniTooltip : MonoBehaviour
{
    [Header("UI References")]
    private Text nameText;
    private Text summaryText;
    private RectTransform body;

    [Header("Runtime Variables")]
    private bool mouseOn = false;
    private float delay = 0;
    private const float DELAY_TIME = 0.1f;
    private RectTransform parent;

    [Header("Singleton")]
    public static MiniTooltip instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        nameText = transform.GetChild(0).GetComponent<Text>();
        summaryText = transform.GetChild(1).GetComponent<Text>();

        body = GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!mouseOn)
        {
            delay -= Time.deltaTime;

            if (delay <= 0)
                gameObject.SetActive(false);
        }
    }

    public void Show(RectTransform icon, string name, string summary)
    {
        transform.SetParent(icon);
        parent = icon;

        int xPosition = (int)icon.position.x;

        if (xPosition >= -3)
            body.localPosition = new Vector3(-170, 0);
        else
            body.localPosition = new Vector3(170, 0);

        body.localScale = new Vector3(1, 1, 1);


        nameText.text = name;
        summaryText.text = summary;

        mouseOn = true;
        gameObject.SetActive(true);
    }

    public void MouseOnIcon(RectTransform icon)
    {
        if (parent == icon)
            mouseOn = true;
    }

    public void MouseOffIcon(RectTransform icon)
    {
        if (parent == icon)
        {
            mouseOn = false;
            delay = 0.1f;
        }
    }
}
