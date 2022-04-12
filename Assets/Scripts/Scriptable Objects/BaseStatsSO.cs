using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Base Stats", fileName = "Entity Base Stats", order = 1)]
public class BaseStatsSO : ScriptableObject       // Race / Objects
{
    [Header("Combat")]
    public int currentHealth;
    public int maxHealth;
    public int defense;
    public int baseMeleeDamage;
    public int baseRangeDamage;


    [Header("Movement")]
    public int movementSpeed;
    public MovementTypes movementType;
}

[CreateAssetMenu(menuName = "ScriptableObjects/Class Stats", fileName = "Class Stats", order = 1)]
public class ClassStatsSO : ScriptableObject              // Class
{
    [Header("Combat")]
    public int bonusHealth;
    public int bonusDefense;
    public int bonusMeleeDamage;
    public int bonusRangeDamage;
}
