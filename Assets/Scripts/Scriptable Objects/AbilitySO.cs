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
    [Space]
    public int abilityValue;
    public GameObject abilityPrefab;
}
