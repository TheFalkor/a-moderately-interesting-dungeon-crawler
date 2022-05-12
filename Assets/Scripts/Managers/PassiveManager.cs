using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveManager : MonoBehaviour
{
    [Header("Runtime Variables")]
    private readonly List<Passive> activePassives = new List<Passive>();
    private GameObject passiveHoverPrefab;
    [SerializeField] private RectTransform passiveHoverParent;

    [Header("Singleton")]
    public static PassiveManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        passiveHoverPrefab = passiveHoverParent.GetChild(0).gameObject;
        passiveHoverPrefab.SetActive(false);
    }

    private void Start()
    {
    }

    public void AddPassive(Passive passive)
    {
        activePassives.Add(passive);

        while (passiveHoverParent.childCount != activePassives.Count)
        {
            Instantiate(passiveHoverPrefab, new Vector3(0, 0, 0), Quaternion.identity, passiveHoverParent);
        }

        Hoverable newHover = passiveHoverParent.GetChild(passiveHoverParent.childCount - 1).GetComponent<Hoverable>();
        newHover.transform.GetChild(0).GetComponent<Image>().sprite = passive.data.passiveSprite;
        newHover.SetInformation(passive.data.passiveName, passive.data.passiveSummary, passive.data.passiveDescription);
        newHover.gameObject.SetActive(true);
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

    public void OnPlayerTakeDamage(Entity enemy)
    {
        foreach (Passive passive in activePassives)
            passive.OnPlayerTakeDamage(enemy);
    }

    public void OnEnemyTakeDamage(Entity enemy)
    {
        foreach (Passive passive in activePassives)
            passive.OnEnemyTakeDamage(enemy);
    }

    public void OnEnemyDeath(Entity enemy)
    {
        foreach (Passive passive in activePassives)
            passive.OnEnemyDeath(enemy);
    }

    public void OnAbilityUsed(AbilityID ability, List<Entity> affectedEnemies = null)
    {
        foreach (Passive passive in activePassives)
            passive.OnAbilityUsed(ability, affectedEnemies);
    }

    public void OnPlayerMove(Player player)
    {
        foreach (Passive passive in activePassives)
            passive.OnPlayerMove(player);
    }

    public void OnPlayerAttack(Player player)
    {
        foreach (Passive passive in activePassives)
            passive.OnPlayerAttack(player);
    }
}
