using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot
{
    public Image image;
    public Text stackText;
    [Space]
    private Color COLOR_FULL = new Color(1, 1, 1, 1);
    private Color COLOR_HALF = new Color(1, 1, 1, 0.2f);
    [Space]
    private readonly GameObject marker;
    private readonly Hoverable hover;

    public InventorySlot(Image image, Text stackText, GameObject marker = null)
    {
        this.image = image;
        this.stackText = stackText;
        this.marker = marker;

        hover = image.transform.parent.GetComponent<Hoverable>();

        ClearSlot();
    }

    public void SetSlot(Item item)
    {
        image.sprite = item.itemSprite;
        if (item.count > 1)
            stackText.text = "" + item.count;
        else
            stackText.text = "";

        if (hover)
        {
            TooltipData data = Inventory.ItemToData(item.data);
            hover.SetInformation(data);
        }

        SetSlotActive(true);
        image.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        image.sprite = null;
        stackText.text = "";

        if (hover)
            hover.ClearInformation();

        SetSlotActive(false);

        image.gameObject.SetActive(false);
    }

    public void SetSlotActive(bool active)
    {
        if (active)
        {
            image.transform.parent.GetComponent<Button>().interactable = true;
            image.color = COLOR_FULL;
        }
        else
        {
            image.transform.parent.GetComponent<Button>().interactable = false;
            image.color = COLOR_HALF;
        }
    }

    public void SelectSlot()
    {
        if (image.sprite != null)
        {
            marker.transform.SetParent(image.transform.parent);
            marker.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            marker.transform.localScale = Vector3.one;
            marker.SetActive(true);
        }
        else
            marker.SetActive(false);
    }
}
