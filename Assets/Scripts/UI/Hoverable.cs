using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Data")]
    public TooltipData data;
    [Space]
    public bool canHover = false;
    private bool miniTooltip = false;

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
                if (miniTooltip)
                    MiniTooltip.instance.Show(body, data);
                else
                    MetaTooltip.instance.Show(body, data);
                mouseOn = false;
                delay = 0;
            }
        }
    }

    public void SetInformation(TooltipData data)
    {
        this.data = data;

        miniTooltip = data.summary != null;
        canHover = data.header != null;
    }

    public void ClearInformation()
    {
        canHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canHover)
        {
            if (miniTooltip)
                MiniTooltip.instance.MouseOnTarget(body);
            else
                MetaTooltip.instance.MouseOnTarget(body);
            mouseOn = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (miniTooltip)
            MiniTooltip.instance.MouseOffIcon(body);
        else
            MetaTooltip.instance.MouseOffTarget(body);

        mouseOn = false;
        delay = 0;
    }
}
