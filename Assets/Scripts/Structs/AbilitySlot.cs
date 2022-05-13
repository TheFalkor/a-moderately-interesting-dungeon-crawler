using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot
{
    public Image image;
    private readonly GameObject marker;
    [Space]
    private Color COLOR_FULL = new Color(1, 1, 1, 1);
    private Color COLOR_HALF = new Color(1, 1, 1, 0.25f);
    [Space]
    private Sprite defaultSprite;
    private Sprite activeBorderSprite;
    private Sprite passiveBorderSprite;

    public AbilitySlot(Image image, GameObject marker = null, Sprite activeBorder = null, Sprite passiveBorder = null)
    {
        this.image = image;
        this.marker = marker;

        defaultSprite = image.transform.parent.GetComponent<Image>().sprite;
        activeBorderSprite = activeBorder;
        passiveBorderSprite = passiveBorder;

        image.gameObject.SetActive(false);
    }

    public void SetSlot(PassiveSO data)
    {
        image.sprite = data.passiveSprite;

        if (AbilityTree.instance.IsPassiveUnlocked(data))
        {
            image.color = COLOR_FULL;
            if (passiveBorderSprite)
                image.transform.parent.GetComponent<Image>().sprite = passiveBorderSprite;
        }
        else
            image.color = COLOR_HALF;

        image.gameObject.SetActive(true);
    }

    public void SetSlot(AbilitySO data)
    {
        image.sprite = data.abilitySprite;

        if (AbilityTree.instance.IsAbilityUnlocked(data))
        {
            image.color = COLOR_FULL;
            if (activeBorderSprite)
                image.transform.parent.GetComponent<Image>().sprite = activeBorderSprite;
        }
        else
            image.color = COLOR_HALF;

        image.gameObject.SetActive(true);
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