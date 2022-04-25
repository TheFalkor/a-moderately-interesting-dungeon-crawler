using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField] private GameObject combatParent;

    private Player player;
    private bool combatActive = false;

    private float combatIntroTimer = 0;

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

    void Start()
    {
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
            foreach (Entity entity in entityList)
            {
                turnQueue.Enqueue(entity);
                entity.ResetTurn();
            }
        }

        if (turnQueue.Peek().Tick(Time.deltaTime))
            turnQueue.Dequeue();
    }

    public void StartCombat(CombatRoomSO room)
    {
        combatParent.SetActive(true);

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        entityList.Add(player);

        GridManager.instance.GenerateCombat(room);

        player.Setup();

        combatIntroTimer = 1.5f;
        combatActive = true;

        // Setup turns and other preparations
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

        turnQueue = newQueue;
    }
}
