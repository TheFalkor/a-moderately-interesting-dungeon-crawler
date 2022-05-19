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
    private bool isDead = false;
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
                StatusEffect eff = activeStatusEffects[i];
                eff.duration--;
                activeStatusEffects[i] = eff;

                if (activeStatusEffects[i].duration == 0)
                {
                    activeStatusEffects.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public virtual void TakeDamage(Damage damage, Occupant attacker)
    {
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return;

        if (isDead)
            return;

        transform.GetChild(0).GetComponent<Animator>().Play("Damage");

        int actualDamage = damage.damage * (1 - (defense / (36 + defense)));

        for (int i = 0; i < activeStatusEffects.Count; i++)
        {
            StatusEffect effect = activeStatusEffects[i];
            if (effect.type == StatusType.FLUTTER)
            {
                actualDamage += (int)(damage.damage * 0.5f);
                activeStatusEffects.Remove(effect);
            }
        }

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(actualDamage, damage.origin);

        if (shield > 0)
        {
            shield -= actualDamage;

            if (shield >= 0)
                return;

            Transform temp = transform.Find("SiphonDeathPlayerShieldVFX(Clone)");
            if (temp)
                Destroy(temp.gameObject);

            actualDamage += shield;
            shield = 0;

            if (actualDamage <= 0)
                return;
        }

        currentHealth -= actualDamage;

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
        if (isDead)
            return;

        transform.GetChild(0).GetComponent<Animator>().Play("Damage");

        int actualDamage = damage * (1 - (defense / (36 + defense)));
        currentHealth -= actualDamage;

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(actualDamage, origin);

        if (currentHealth <= 0)
            Death();
    }

    public virtual void AddStatusEffect(StatusEffect statusEffect)
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

    public virtual void RemoveStatusEffect(StatusEffect statusEffect)
    {
        activeStatusEffects.Remove(statusEffect);
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
        tile.AttackTile(damage, this);
    }

    protected virtual void Death()
    {
        isDead = true;
        transform.GetChild(0).GetComponent<Animator>().Play("Death");

        CombatManager.instance.RemoveOccupant(this);

        currentTile.ClearHighlight();
        Destroy(gameObject, 4 /15f);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}