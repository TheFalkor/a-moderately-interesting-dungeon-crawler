using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{
    [SerializeField] private Sprite healthIcon;
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite defenseIcon;
    [SerializeField] private Sprite movementIcon;
    [SerializeField] private Sprite actionIcon;

    public static IconManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    public Sprite GetSprite(string name)
    {
        switch (name)
        {
            case "HP": return healthIcon;
            case "ATK": return attackIcon;
            case "DEF": return defenseIcon;
            case "AP": return actionIcon;
            case "MP": return movementIcon;
        }

        Debug.LogWarning("IconManager :: Could not locate sprite: " + name);
        return null;
    }
}
