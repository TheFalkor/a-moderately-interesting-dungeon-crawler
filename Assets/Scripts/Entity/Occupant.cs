using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Occupant : MonoBehaviour
{
    [Header("Stats Reference")]
    [SerializeField] protected BaseStatsSO baseStat;


    protected Tile currentTile;
    protected int currentHealth;
    protected int maxhealth;
    protected int defense;
    protected int baseMeleeDamage;
    protected int baseRangeDamage;
    protected DamageOrigin originType;
    private readonly List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    // StatusType immunity list

    protected SpriteRenderer render;


    public virtual void Initialize()
    {
        currentHealth = baseStat.currentHealth;
        maxhealth = baseStat.maxHealth;
        defense = baseStat.defense;
        baseMeleeDamage = baseStat.baseMeleeDamage;
        baseRangeDamage = baseStat.baseRangeDamage;
        originType = baseStat.origin;

        currentTile = GridManager.instance.GetTileWorld(transform.position);
        currentTile.SetOccupant(this);

        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        render.sortingOrder = currentTile.GetPosition().y + 2;
    }

    public virtual void TakeDamage(Damage damage)
    {
        Debug.Log(damage.damage + transform.name);
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return; 

        currentHealth -= damage.damage;

        if (currentHealth <= 0)
        {
            Death();
            return;
        }

        if (damage.statusEffects == null)
            return;

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

    public virtual void Heal(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Min(currentHealth, maxhealth);
    }

    protected void Attack(Tile tile, Damage damage)
    {
        tile.AttackTile(damage);
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

}