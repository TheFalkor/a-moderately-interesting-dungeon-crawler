using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField] private GameObject combatParent;

    private Player player;
    private bool combatActive = false;

    private float combatIntroTimer = 0;

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

        player.Tick(Time.deltaTime);
    }

    public void StartCombat(CombatRoomSO room)
    {
        combatParent.SetActive(true);

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GridManager.instance.GenerateCombat(room);

        player.Setup();

        combatIntroTimer = 1.5f;
        combatActive = true;
        // Setup turns and other preparations
    }
}
