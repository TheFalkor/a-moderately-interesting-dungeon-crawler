using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Events")]
    public OnScriptDelegate<Entity> OnPreTurn;
    public OnScriptDelegate<Entity> OnEndTurn;
    public OnScriptDelegate<Entity> OnPlayerTakeDamage;
    public OnScriptDelegate<Entity> OnEnemyTakeDamage;
    public OnScriptDelegate<Entity> OnEnemyDeath;
    public OnScriptDelegate<Player> OnPlayerMove;
    public OnScriptDelegate<Player> OnPlayerAttack;
    public OnAbilityDelegate OnAbilityUsed;


    public delegate void OnTriggerDelegate();
    public delegate void OnIntDelegate(int value);
    public delegate void OnBoolDelegate(bool value);
    public delegate void OnFloatDelegate(float value);
    public delegate void OnGameObjectDelegate(GameObject value);
    public delegate void OnScriptDelegate<TScript>(TScript script);
    public delegate void OnAbilityDelegate(AbilityID ability, List<Entity> affectedEnemies = null);


    private void Awake()
    {
        ServiceLocator.Register(this);
    }
}
