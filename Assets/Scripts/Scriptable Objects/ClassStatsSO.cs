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
    
    [Header("Ability")]
    public List<AbilitySO> startingAbilities;
    public List<AbilitySO> unlockableAbilities;
    
    [Header("Passive")]
    public List<PassiveSO> startingPassives;
    public List<PassiveSO> unlockablePassives;
}