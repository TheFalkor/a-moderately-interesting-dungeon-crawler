using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Base Stats", fileName = "Entity Base Stats", order = 1)]
public class BaseStatsSO : ScriptableObject       // Race / Objects
{
    [Header("Information")]
    public string entityName;
    public string entityDescription;
    public Sprite entitySprite;

    [Header("Stamina")]
    public int actionPoints;
    public int movementPoints;
    public MovementTypes movementType;

    [Header("Combat")]
    public int maxHealth;
    public int defense;
    public int baseMeleeDamage;
    public int baseRangeDamage;
    [Space]
    public DamageOrigin origin;
}
