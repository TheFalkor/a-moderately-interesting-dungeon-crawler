using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public enum EnemyBehaviourType
    {
        SIMPLE_MELEE,
        TestDoNotSelect
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

        Sensing = new EnemySensing(this);

        switch(behaviourType)
        {
            case EnemyBehaviourType.SIMPLE_MELEE:
                Behaviour = new EnemySimpleMeleeBehaviour(Sensing);
                break;

            default:
                Debug.LogError("Enemy in the scene doesn't have a behaviour set");
                Behaviour = new EnemySimpleMeleeBehaviour(Sensing);
                break;
        }

        Behaviour.Initialize();

        actionsQueue = new Queue<Action>();
    }

    public override void PreTurn()
    {
        base.PreTurn();

        actionsQueue = Behaviour.DecideTurn();
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
                Move(currentAction.direction);
                break;

            case ActionType.MELEE_ATTACK:
                MeleeAttack(currentAction);
                break;

            case ActionType.RANGED_ATTACK:
                Debug.Log("Insanely Amazing Ranged Attack!");
                break;

            case ActionType.PEBBLE:
                StartCoroutine(Pebble());
                Debug.Log("PEBBLE!!!!!!!!!!!!!!");
                break;
        }
    }

    private void MeleeAttack(Action action)
    {
        // TO DO : check the weapon of the enemy

        switch(action.direction)
        {
            case Direction.NORTH:
                AttackWithNone(GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.down));
                break;
            case Direction.EAST:
                AttackWithNone(GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.right));
                break;
            case Direction.SOUTH:
                AttackWithNone(GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.up));
                break;
            case Direction.WEST:
                AttackWithNone(GridManager.instance.GetTile(currentTile.GetPosition() + Vector2Int.left));
                break;
        }

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
            Debug.Log("Waiting for Pebble");
            waitHappened = true;
            yield return new WaitForSeconds(0.15f);
        }

        Debug.Log("FIRING PEBBLE LESS GOOOO");
        Attack(Sensing.player.currentTile, new Damage(baseRangeDamage, originType));
    }
}
