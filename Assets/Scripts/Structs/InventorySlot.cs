using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot
{
    public Image image;
    public Text stackText;
    private GameObject marker;

    public InventorySlot(Image image, Text stackText, GameObject marker = null)
    {
        this.image = image;
        this.stackText = stackText;
        this.marker = marker;

        ClearSlot();
    }

    public void SetSlot(Item item)
    {
        image.sprite = item.itemSprite;
        if (item.count > 1)
            stackText.text = "" + item.count;
        else
            stackText.text = "";

        image.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        image.sprite = null;
        stackText.text = "";

        image.gameObject.SetActive(false);
    }

    public void SelectSlot()
    {
        if (image.sprite != null)
        {
            marker.transform.SetParent(image.transform.parent);
            marker.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            marker.SetActive(true);
        }
        else
            marker.SetActive(false);
    }
}
