using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Class Stats", fileName = "Class Stats", order = 1)]
public class ClassStatsSO : ScriptableObject
{
    [Header("Information")]
    public string className;
    public string classDescription;

    [Header("Combat")]
    public int bonusHealth;
    public int bonusDefense;
    public int bonusMeleeDamage;
    public int bonusRangeDamage;
    [Space]
    public List<AbilitySO> abilities;
}