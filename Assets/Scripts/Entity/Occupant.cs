using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Occupant : MonoBehaviour
{
    [Header("Stats Reference")]
    [SerializeField] protected BaseStatsSO baseStat;
    [SerializeField] private GameObject damagePopup;

    [Header("Occupant Stats")]
    protected int currentHealth;
    protected int maxhealth;
    protected int defense;
    protected int baseMeleeDamage;
    protected int baseRangeDamage;
    protected DamageOrigin originType;

    [Header("Runtime Variables")]
    protected SpriteRenderer render;
    protected Tile currentTile;
    private readonly List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    protected Inventory inventory;  //curently only player use inventory. 
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
        render.sortingOrder = currentTile.GetPosition().y - 10;
    }

    public virtual void TakeDamage(Damage damage)
    {
        if (originType == damage.origin && damage.origin != DamageOrigin.NEUTRAL)
            return; 

        currentHealth -= damage.damage;

        Instantiate(damagePopup, transform.position + new Vector3(0, 0.5f), Quaternion.identity).GetComponent<DamagePopup>().Setup(damage.damage, damage.origin == DamageOrigin.FRIENDLY);

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
    public void GiveItem(InventoryItem item)
    {
        if (inventory != null)
        {
            inventory.AddItem(item);
        }
    }

    public virtual void UseItem(int index)
    {
        if (inventory != null)
        {
            inventory.UseItem(index);
        }

    }
    protected virtual void UpdateStats() 
    {
        
        maxhealth = baseStat.maxHealth;
        defense = baseStat.defense;
        baseMeleeDamage = baseStat.baseMeleeDamage;
        if (inventory != null) 
        {
            if (inventory.HasEquipmentInventory()) 
            {
                maxhealth += inventory.EquipedItemsStatValue(StatType.MAX_HEALTH);
                defense += inventory.EquipedItemsStatValue(StatType.DEFENSE);
                baseMeleeDamage += inventory.EquipedItemsStatValue(StatType.ATTACK);
            }
        }
        //baseRangeDamage = baseStat.baseRangeDamage;
    }
}