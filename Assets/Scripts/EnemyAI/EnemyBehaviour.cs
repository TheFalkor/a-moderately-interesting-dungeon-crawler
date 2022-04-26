using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    public abstract void Initialize();
    public abstract Queue<Action> DecideTurn();
}
