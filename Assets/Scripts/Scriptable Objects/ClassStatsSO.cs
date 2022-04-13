using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Class Stats", fileName = "Class Stats", order = 1)]
public class ClassStatsSO : ScriptableObject
{
    [Header("Combat")]
    public int bonusHealth;
    public int bonusDefense;
    public int bonusMeleeDamage;
    public int bonusRangeDamage;
}