using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Passive", fileName = "Passive Name", order = 5)]
public class PassiveSO : ScriptableObject
{
    [Header("Passive Information")]
    public string passiveName;
    public string passiveSummary;
    public string passiveDescription;
    [Space]
    public Sprite passiveSprite;
    [Space]
    public PassiveID passiveType;

    [Header("Passive Data")]
    public int passiveValue;
    public GameObject tileEffect;
    public List<GameObject> passiveVFX;
}
