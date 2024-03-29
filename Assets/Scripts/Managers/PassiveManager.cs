using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveManager : MonoBehaviour
{
    [Header("Runtime Variables")]
    private readonly List<Passive> activePassives = new List<Passive>();
    private readonly List<Hoverable> passiveHoverList = new List<Hoverable>();
    [SerializeField] private RectTransform passiveHoverParent;

    [Header("Singleton")]
    public static PassiveManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        foreach (Transform go in passiveHoverParent)
        { 
            passiveHoverList.Add(go.GetComponent<Hoverable>());
            go.gameObject.SetActive(false);
        }
    }

    public void AddPassive(Passive passive)
    {
        activePassives.Add(passive);

        foreach (Hoverable hover in passiveHoverList)
        {
            if (hover.gameObject.activeSelf)
                continue;

            TooltipData data = new TooltipData();
            data.header = passive.data.passiveName;
            data.description = passive.data.passiveDescription;
            data.rightHeader = "PASSIVE";

            hover.transform.GetChild(0).GetComponent<Image>().sprite = passive.data.passiveSprite;
            hover.SetInformation(data);
            hover.gameObject.SetActive(true);
            return;
        }
    }

    public void RemovePassive(PassiveSO data)
    {
        for (int i = 0; i < activePassives.Count; i++)
        {
            if (activePassives[i].data == data)
            {
                activePassives.RemoveAt(i);
                for (int j = 0; j < passiveHoverParent.childCount; j++)
                {
                    GameObject child = passiveHoverParent.GetChild(j).gameObject;

                    if (child.transform.GetChild(0).GetComponent<Image>().sprite == data.passiveSprite)
                    {
                        child.SetActive(false);
                        child.transform.SetAsLastSibling();
                    }
                }
                break;
            }
        }
    }
}
