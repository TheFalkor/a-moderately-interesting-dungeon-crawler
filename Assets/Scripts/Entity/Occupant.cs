using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupant : MonoBehaviour
{
    [Header("Stats Reference")]
    [SerializeField] protected BaseStatsSO baseStat;


    protected Tile currentTile;
    private int maxhealth;
    private int currentHealth;
    private int defense;
    private DamageOrigin occupantType;
    private List<StatusEffect> statusList = new List<StatusEffect>();
    // StatusType immunity list


    public void Start()
    {
        //maxhealth = baseStat.maxHealth;
        //currentHealth = maxhealth;
        //defense = baseStat.defense;

        currentTile = GridManager.instance.GetTileWorld(transform.position);
    }

    public void TakeDamage(Damage damage)
    {
        if (occupantType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
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
            for (int i = 0; i < statusList.Count; i++)
            {
                if (dse.type == statusList[i].type)
                {
                    if (dse.statusDuration > statusList[i].statusDuration)
                        statusList[i] = dse;

                    found = true;
                    break;
                }
            }

            if (!found)
                statusList.Add(dse);
        }
    }

}