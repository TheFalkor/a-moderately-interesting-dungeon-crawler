using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private enum EnemyBehaviourType
    {
        SIMPLE_MELEE,
        DUMB,
        SWORD,
        SPEAR
    }

    [SerializeField]
    private EnemyBehaviourType behaviourType;
    private EnemyBehaviour Behaviour;
    private EnemySensing Sensing;
    public GameObject PebblePrefab;

    private Action currentAction;
    private Queue<Action> actionsQueue;

    private float waitTimer = 0;
    private const float ACTION_WAIT_TIME = 0.2f;

    public override void Initialize()
    {
        base.Initialize();

        transform.localScale = new Vector3(-1f, 1f);

        Sensing = new EnemySensing(this);

        switch(behaviourType)
        {
            case EnemyBehaviourType.SIMPLE_MELEE:
                Behaviour = new EnemySimpleMeleeBehaviour();
                break;

            case EnemyBehaviourType.DUMB:
                Behaviour = new EnemyDumbBehaviour();
                break;

            case EnemyBehaviourType.SWORD:
                Behaviour = new EnemySwordBehaviour();
                break;

            default:
                Debug.LogError("Enemy in the scene doesn't have a behaviour set");
                Behaviour = new EnemySimpleMeleeBehaviour();
                break;
        }

        Behaviour.Initialize(Sensing);

        actionsQueue = new Queue<Action>();
    }

    public override void PreTurn()
    {
        base.PreTurn();

        bool allowMovement = true;

        foreach (StatusEffect sf in activeStatusEffects)
            if (sf.type == StatusType.ROOTED)
            {
                allowMovement = false;
                break;
            }

        actionsQueue = Behaviour.DecideTurn(currentActionPoints, currentMovementPoints, allowMovement);
    }

    public override bool Tick(float deltaTime)
    {
        if (IsBusy())
            return false;

        if (actionsQueue.Count == 0)
            return true;

        if (waitTimer >= ACTION_WAIT_TIME)
        {
            waitTimer = 0;
            QueueAction();
        }

        else
            waitTimer += deltaTime;

        return false;
    }

    private void QueueAction()
    {
        currentAction = actionsQueue.Dequeue();

        switch (currentAction.action)
        {
            case ActionType.MOVE:
                EnemyMove(currentAction.target);
                break;

            case ActionType.MELEE_ATTACK:
                MeleeAttack(currentAction);
                break;

            case ActionType.RANGED_ATTACK:
                Debug.Log("Insanely Amazing Ranged Attack!");
                break;

            case ActionType.PEBBLE:
                StartCoroutine(Pebble());
                break;
        }
    }

    private void EnemyMove(Tile target)
    {
        Vector2Int direction = target.GetPosition() - currentTile.GetPosition();

        if (direction == Vector2Int.down)
            Move(Direction.NORTH);
        else if (direction == Vector2Int.right)
            Move(Direction.EAST);
        else if (direction == Vector2Int.up)
            Move(Direction.SOUTH);
        else if (direction == Vector2Int.left)
            Move(Direction.WEST);
        else
            Debug.LogError("ENEMY MOVE BROKE AAAAAAAAAAAA (invalid target)");
    }

    private void MeleeAttack(Action action)
    {

        transform.GetChild(0).GetComponent<Animator>().Play("Attack");

        Damage damage = new Damage(meleeDamage, originType);

        Attack(action.target, damage);

        return;
    }

    IEnumerator Pebble()
    {
        bool waitHappened = false;

        GameObject pebble = Instantiate(PebblePrefab);
        pebble.transform.position = transform.position;
        pebble.transform.position += new Vector3(0, 0.5f, 0);
        pebble.GetComponent<Projectile>().setTarget(Sensing.player.transform.position);


        while (!waitHappened)
        {
            waitHappened = true;
            yield return new WaitForSeconds(0.15f);
        }

        Attack(Sensing.player.currentTile, new Damage(rangeDamage, originType));
    }

    public override void TakeDamage(Damage damage, Occupant attacker = null)
    {
        base.TakeDamage(damage);

        PassiveManager.instance.OnEnemyTakeDamage(this);
    }

    protected override void Death()
    {
        PassiveManager.instance.OnEnemyDeath(this);

        base.Death();
    }
}
