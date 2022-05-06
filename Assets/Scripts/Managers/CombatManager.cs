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

    private List<Occupant> occupantList = new List<Occupant>();
    private List<Entity> entityList = new List<Entity>();
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
            turnQueue.Dequeue();

            if (turnQueue.Count > 0)
                turnQueue.Peek().PreTurn();
        }
    }

    public void StartCombat(CombatRoomSO room)
    {
        entityList.Clear();
        turnQueue.Clear();

        combatParent.SetActive(true);

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        entityList.Add(player);

        GridManager.instance.GenerateCombat(room);


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
}
