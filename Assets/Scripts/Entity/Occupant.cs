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
    protected int baseMeleeDamage;
    protected int baseRangeDamage;
    [HideInInspector] public DamageOrigin originType;
    
    [Header("Runtime Variables")]
    protected SpriteRenderer render;
    [HideInInspector] public Tile currentTile;
    private readonly List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    
    // StatusType immunity list


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

    public void UpdateStatusEffects()
    {
        // Tick all status effects
        // note: effect.duration == 0, remove it (duration == -1 is permanent)
    }

    public virtual void TakeDamage(Damage damage)
    {
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return;

        if (currentHealth <= 0)
            return;

        currentHealth -= damage.damage;

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(damage.damage, damage.origin);

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

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        bool found = false;
        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            if (statusEffect.type == activeStatusEffects[i].type)
            {
                if (statusEffect.duration > activeStatusEffects[i].duration)
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