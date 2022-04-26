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

    private Action currentAction;
    private Queue<Action> actionsQueue;

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

        currentAction = actionsQueue.Dequeue();

        switch (currentAction.action)
        {
            case ActionType.MOVE:
                Move(currentAction.direction);
                break;

            case ActionType.MELEEATTACK:
                Debug.Log("Super Cool Melee Attack!");
                break;

            case ActionType.RANGEDATTACK:
                Debug.Log("Insanely Amazing Ranged Attack!");
                break;

            case ActionType.PEBBLE:
                Debug.Log("PEBBLE!!!!!!!!!!!!!!");
                break;
        }

        return false;
    }
}
