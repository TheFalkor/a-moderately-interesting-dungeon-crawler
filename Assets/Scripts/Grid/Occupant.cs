using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupant : MonoBehaviour
{
    [SerializeField] protected EntityStatsSO raceStat;
    [SerializeField] protected ClassStats classStat;
    private int maxhealth;
    private int currentHealth;
    private int defense;
    private DamageOrigin occupantType;
    private List<StatusEffect> statusList = new List<StatusEffect>();
    // StatusType immunity list

    private void Start()
    {
        maxhealth = raceStat.maxHealth;
        currentHealth = maxhealth;
        defense = raceStat.defense;
    }

    public void TakeDamage(Damage damage)
    {
        if (occupantType == damage.dealer && damage.dealer != DamageOrigin.NEUTRAL)
            return; 

        currentHealth -= damage.damage;

        if (currentHealth <= 0)
        {
            // Death screech
            // if statusrefect == boom -> then boom
            
            Destroy(gameObject);
            return;
        }

        foreach (StatusEffect dse in damage.statusList)
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