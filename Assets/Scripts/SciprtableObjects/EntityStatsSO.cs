using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/EntityStat", fileName = "EntityStat", order = 1)]
public class EntityStatsSO : ScriptableObject
{
    [Header("Combat")]
    public int currentHealth;
    public int maxHealth;
    public int defense;
    public int baseMeleeDamage;
    public int baseRangeDamage;
    [Range(0f, 1f)]
    public float critChance;
    [Range(0f, 1f)]
    public float missChance;


    [Header("Movement")]
    public int movementSpeed;
    public MovementTypes movementType;
}

[CreateAssetMenu(menuName = "ScriptableObjects/ClassStats", fileName = "ClassStats", order = 1)]
public class ClassStats : ScriptableObject      // ?
{
    [Header("Combat")]
    public int bonusHealth;
    public int bonusDefense;
    public int bonusMeleeDamage;
    public int bonusRangeDamage;
    [Range(0f, 1f)]
    public float critChance;
    [Range(0f, 1f)]
    public float missChance;


    [Header("Movement")]
    public int movementSpeed;
    public MovementTypes movementType;
}
