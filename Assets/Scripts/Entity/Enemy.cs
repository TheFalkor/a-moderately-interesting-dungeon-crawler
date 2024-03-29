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
    [SerializeField] private GameObject attackVFX;
    [SerializeField]
    private float splashMultiplier;

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

        maxhealth = (int)(maxhealth * ConsistentData.difficultyScale);
        currentHealth = maxhealth;
        baseMeleeDamage = (int)(baseMeleeDamage * ConsistentData.difficultyScale);

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

            case EnemyBehaviourType.SPEAR:
                Behaviour = new EnemySpearBehaviour();
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

            case ActionType.SPLASH_ATTACK:
                MeleeAttack(currentAction, true);
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
        audioCore.PlaySFX("ENEMY_MOVE");
    }

    private void MeleeAttack(Action action, bool isSplash = false)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Animator>().Play("Attack");
        Damage damage;

        if (isSplash)
            damage = new Damage(Mathf.RoundToInt(meleeDamage * splashMultiplier), originType);
        else
            damage = new Damage(meleeDamage, originType);

        Attack(action.target, damage);
        GameObject vfx;

        switch (behaviourType)
        {
            case EnemyBehaviourType.DUMB:
                vfx = Instantiate(attackVFX, action.target.transform.position, Quaternion.identity);
                Destroy(vfx, 0.5f);
                break;
            case EnemyBehaviourType.SPEAR:
                Vector3 direction = action.target.transform.position - transform.position;
                direction.Normalize();

                vfx = Instantiate(attackVFX, transform.position + direction * 1.3f, Quaternion.identity);

                if (direction.y == 0)
                    vfx.transform.position += new Vector3(0, 0.5f);
                vfx.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
                
                Destroy(vfx, 9 / 15f);
                break;
            case EnemyBehaviourType.SWORD:
                vfx = Instantiate(attackVFX, action.target.transform.position, Quaternion.identity);
                Destroy(vfx, 0.5f);
                break;
        }

        audioCore.PlaySFX("SLASH");

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
        audioCore.PlaySFX("SLASH");
    }

    public override void TakeDamage(Damage damage, Occupant attacker = null)
    {
        ServiceLocator.Get<EventManager>().OnEnemyTakeDamage?.Invoke(this);

        base.TakeDamage(damage, this);
    }

    protected override void Death()
    {
        ServiceLocator.Get<EventManager>().OnEnemyDeath?.Invoke(this);
        audioCore.PlaySFX("ENEMY_DEATH");
        base.Death();
    }
}
