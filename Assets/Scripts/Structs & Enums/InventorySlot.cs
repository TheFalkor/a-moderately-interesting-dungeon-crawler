using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot
{
    public Image image;
    public Text stackText;

    public InventorySlot(Image image, Text stackText)
    {
        this.image = image;
        this.stackText = stackText;

        ClearSlot();
    }

    public void SetSlot(Item item)
    {
        image.sprite = item.itemSprite;
        stackText.text = "" + item.count;

        image.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        image.sprite = null;
        stackText.text = "";

        image.gameObject.SetActive(false);
    }
}
