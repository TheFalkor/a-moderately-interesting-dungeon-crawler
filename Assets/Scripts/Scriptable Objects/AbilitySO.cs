using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability", fileName = "Ability Name", order = 4)]
public class AbilitySO : ScriptableObject
{
    [Header("Ability Information")]
    public string abilityName;
    public string abilitySummary;
    public string abilityDescription;
    [Space]
    public Sprite abilitySprite;
    [Space]
    public AbilityID abilityType;

    [Header("Ability Data")]
    public int cooldown;
    public int actionPointCost;
    [Space]
    public float abilityValue;
    public GameObject abilityPrefab;
    [Space]
    public GameObject[] abilityVFX;
}
