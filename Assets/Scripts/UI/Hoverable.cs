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
                MetaTooltip.instance.Show(body, data);
                // MiniTooltip.instance.Show(body, header, summary);
                mouseOn = false;
                delay = 0;
            }
        }
    }

    public void SetInformation(TooltipData data)
    {
        this.data = data;

        canHover = true;
    }

    public void ClearInformation()
    {
        canHover = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canHover)
        {
            MetaTooltip.instance.MouseOnTarget(body);
            mouseOn = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MetaTooltip.instance.MouseOffTarget(body);

        mouseOn = false;
        delay = 0;
    }
}
