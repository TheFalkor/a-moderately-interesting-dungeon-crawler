using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject
{
    [Header("Item Data")]
    public string itemName;
    public string itemDescription;
    [Space]
    public Sprite itemSprite;
    [Space]
    public StackSize stackSize;
    [HideInInspector] public ItemType itemType;
}