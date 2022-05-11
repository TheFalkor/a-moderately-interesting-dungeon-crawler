using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Occupant : MonoBehaviour
{
    [Header("Combat Effects")]
    [SerializeField] private GameObject damagePopup;

    [Header("Stats Reference")]
    public BaseStatsSO baseStat;

    [Header("Occupant Stats")]
    protected int currentHealth;
    protected int maxhealth;
    protected int defense;
    [HideInInspector] public int shield = 0;
    protected int baseMeleeDamage;
    protected int baseRangeDamage;
    [HideInInspector] public DamageOrigin originType;
    
    [Header("Runtime Variables")]
    protected SpriteRenderer render;
    [HideInInspector] public Tile currentTile;
    public readonly List<StatusEffect> activeStatusEffects = new List<StatusEffect>();


    public virtual void Initialize()
    { 
        maxhealth = baseStat.maxHealth;
        currentHealth = maxhealth;
        defense = baseStat.defense;
        baseMeleeDamage = baseStat.baseMeleeDamage;
        baseRangeDamage = baseStat.baseRangeDamage;
        originType = baseStat.origin;
        
        currentTile = GridManager.instance.GetTileWorld(transform.position);
        currentTile.SetOccupant(this);

        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        UpdateLayerIndex();
    }

    public virtual void UpdateStatusEffects()
    {
        // Foreach if needed

        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            if (activeStatusEffects[i].duration != -1)
            {
                activeStatusEffects[i].DecreaseDuration();

                if (activeStatusEffects[i].duration == 0)
                {
                    activeStatusEffects.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public virtual void TakeDamage(Damage damage)
    {
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return;

        if (currentHealth <= 0)
            return;

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(damage.damage, damage.origin);
        
        if (shield > 0)
        {
            shield -= damage.damage;

            if (shield >= 0)
                return;

            damage.damage += shield;

            if (damage.damage <= 0)
                return;
        }

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
            AddStatusEffect(dse);
        }
    }

    public virtual void TakeCleanDamage(int damage, DamageOrigin origin)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(damage, origin);

        if (currentHealth <= 0)
            Death();
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        bool found = false;
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            if (statusEffect.type == activeStatusEffects[i].type)
            {
                if (statusEffect.duration > activeStatusEffects[i].duration && activeStatusEffects[i].duration != -1)
                    activeStatusEffects[i] = statusEffect;

                found = true;
                break;
            }
        }

        if (!found)
            activeStatusEffects.Add(statusEffect);
    }

    public void UpdateLayerIndex()
    {
        render.sortingOrder = currentTile.GetPosition().y * 5 - 90;
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
        CombatManager.instance.RemoveOccupant(this);
        Destroy(gameObject);
    }
}