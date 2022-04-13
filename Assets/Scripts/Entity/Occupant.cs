using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupant : MonoBehaviour
{
    [Header("Stats Reference")]
    [SerializeField] protected BaseStatsSO baseStat;


    [HideInInspector] public Tile currentTile;
    protected int currentHealth;
    protected int maxhealth;
    protected int defense;
    protected int baseMeleeDamage;
    protected int baseRangeDamage;
    protected DamageOrigin originType;
    private readonly List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    // StatusType immunity list


    public void Start()
    {
        currentHealth = maxhealth;
        maxhealth = baseStat.maxHealth;
        defense = baseStat.defense;
        baseMeleeDamage = baseStat.baseMeleeDamage;
        baseRangeDamage = baseStat.baseRangeDamage;
        originType = baseStat.origin;

        currentTile = GridManager.instance.GetTileWorld(transform.position);
    }

    public void TakeDamage(Damage damage)
    {
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return; 

        currentHealth -= damage.damage;

        if (currentHealth <= 0)
        {
            // Death screech
            // if statusrefect == boom -> then boom
            
            Destroy(gameObject);
            return;
        }

        foreach (StatusEffect dse in damage.statusEffects)
        {
            bool found = false;
            for (int i = 0; i < activeStatusEffects.Count; i++)
            {
                if (dse.type == activeStatusEffects[i].type)
                {
                    if (dse.duration > activeStatusEffects[i].duration)
                        activeStatusEffects[i] = dse;

                    found = true;
                    break;
                }
            }

            if (!found)
                activeStatusEffects.Add(dse);
        }
    }

    public void Attack()
    {

    }

}