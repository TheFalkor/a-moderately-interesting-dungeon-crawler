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
    [Space]
    public DamageOrigin origin;


    [Header("Movement")]
    public int movementSpeed;
    public MovementTypes movementType;
}
