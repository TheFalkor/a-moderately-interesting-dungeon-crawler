using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Data")]
    public string header;
    public string summary;
    public string description;
    [Space]
    public bool canHover = false;

    [Header("Runtime Variables")]
    private RectTransform body;
    private bool mouseOn = false;
    private float delay = 0;


    void Start()
    {
        body = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (mouseOn)
        {
            delay += Time.deltaTime;

            if (delay > 0.25f)
            {
                MiniTooltip.instance.Show(body, header, summary);
                mouseOn = false;
                delay = 0;
            }
        }
    }

    public void SetInformation(string header, string summary)
    {
        this.header = header;
        this.summary = summary;

        canHover = header != "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canHover)
        {
            MiniTooltip.instance.MouseOnIcon(body);
            mouseOn = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MiniTooltip.instance.MouseOffIcon(body);

        mouseOn = false;
        delay = 0;
    }
}
