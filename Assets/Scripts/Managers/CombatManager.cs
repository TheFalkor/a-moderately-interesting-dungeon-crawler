using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField] private GameObject combatParent;
    [SerializeField] private Animator transitionAnimator;

    private Player player;
    private bool combatActive = false;

    private float combatIntroTimer = 0;

    private List<TileEffect> tileEffectList = new List<TileEffect>();
    [HideInInspector] public List<Occupant> occupantList = new List<Occupant>();
    [HideInInspector] public List<Entity> entityList = new List<Entity>();
    private Queue<Entity> turnQueue = new Queue<Entity>();

    [Header("Singleton")]
    public static CombatManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }


    void Update()
    {
        if (!combatActive)
            return;

        if (combatIntroTimer > 0)
        {
            combatIntroTimer -= Time.deltaTime;
            return;
        }

        if (turnQueue.Count == 0)
        {
            GridManager.instance.ClearAllHighlights();

            AbilityManager.instance.DecreaseAbilityCooldown();

            foreach (TileEffect effect in tileEffectList)
                effect.PreTurn();

            for (int i = 0; i < tileEffectList.Count; i++)
            {
                if (tileEffectList[i].duration == 0)
                {
                    tileEffectList[i].OnDespawn();
                    i--;
                }
            }

            foreach (Occupant occ in occupantList)
                occ.UpdateStatusEffects();

            player.PreTurn();

            foreach (Entity entity in entityList)
            {
                turnQueue.Enqueue(entity);
            }
        }

        if (turnQueue.Peek().Tick(Time.deltaTime))
        {
            PassiveManager.instance.OnEndTurn(turnQueue.Peek());

            turnQueue.Dequeue();

            if (turnQueue.Count > 0)
                turnQueue.Peek().PreTurn();
        }
    }

    public void StartCombat(CombatRoomSO room)
    {
        entityList.Clear();
        turnQueue.Clear();

        if (CombatUI.instance)
            CombatUI.instance.UpdateAbilityUI();


        combatParent.SetActive(true);

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GridManager.instance.GenerateCombat(room);
        entityList.Add(player);
        entityList.Reverse();
        player.ResetPlayer();


        combatIntroTimer = 0.5f;
        combatActive = true;

        transitionAnimator.SetBool("Closed", false);

        // Setup turns and other preparations
    }

    public void HideRoom()
    {
        combatParent.SetActive(false);

        foreach (Occupant occ in occupantList)
            Destroy(occ.gameObject);

        while (tileEffectList.Count > 0)
        {
            tileEffectList[0].OnDespawn();
        }

        tileEffectList.Clear();
        occupantList.Clear();
    }

    public void AddEntity(Entity entity)
    {
        entityList.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        entityList.Remove(entity);

        Queue<Entity> newQueue = new Queue<Entity>();

        foreach (Entity e in entityList)
        {
            if (turnQueue.Contains(e))
                newQueue.Enqueue(e);
        }

        if (entityList.Count == 1)
        {
            combatActive = false;
            AbilityManager.instance.ResetAbilityCooldown();
            VictoryManager.instance.ShowPopup();
        }

        turnQueue = newQueue;
    }

    public void AddOccupant(Occupant occupant)
    {
        occupantList.Add(occupant);
    }

    public void RemoveOccupant(Occupant occupant)
    {
        occupantList.Remove(occupant);
    }
	
	public void AddTileEffect(TileEffect tileEffect)
    {
        tileEffectList.Add(tileEffect);
    }

    public void RemoveTileEffect(TileEffect tileEffect)
    {
        tileEffectList.Remove(tileEffect);
    }
}
