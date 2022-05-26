using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Base Stats", fileName = "Entity Base Stats", order = 1)]
public class BaseStatsSO : ScriptableObject       // Race / Objects
{
    [Header("Information")]
    public string entityName;
    public string entityDescription;
    public Sprite entitySprite;
    public Sprite overheadIconSprite;

    [Header("Race Specifics")]
    public Sprite racePortrait;
    public Sprite miniSprite;

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

    [Header("Ability")]
    public List<AbilitySO> startingAbilities;
    public List<AbilitySO> unlockableAbilities;
    
    [Header("Passive")]
    public List<PassiveSO> unlockablePassives;

}
