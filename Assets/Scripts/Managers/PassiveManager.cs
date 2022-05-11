using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    public List<PassiveSO> testPassives = new List<PassiveSO>();

    [Header("Runtime Variables")]
    private readonly List<Passive> activePassives = new List<Passive>();

    [Header("Singleton")]
    public static PassiveManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void Start()
    {
        activePassives.Add(new FlutterSpread());
    }

    public void AddPassive(Passive passive)
    {
        activePassives.Add(passive);

        // Update UI
    }

    public void OnPreTurn(Entity entity)
    {
        foreach (Passive passive in activePassives)
            passive.OnPreTurn(entity);
    }

    public void OnEndTurn(Entity entity)
    {
        foreach (Passive passive in activePassives)
            passive.OnEndTurn(entity);
    }

    public void OnPlayerTakeDamage(Player player)
    {
        foreach (Passive passive in activePassives)
            passive.OnPlayerTakeDamage(player);
    }

    public void OnEnemyTakeDamage(Entity enemy)
    {
        foreach (Passive passive in activePassives)
            passive.OnEnemyTakeDamage(enemy);
    }

    public void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        foreach (Passive passive in activePassives)
            passive.OnAbilityUsed(ability, affectedEnemies);
    }
}
