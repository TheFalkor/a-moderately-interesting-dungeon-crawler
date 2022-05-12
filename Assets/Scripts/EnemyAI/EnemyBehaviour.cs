using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    public abstract void Initialize(EnemySensing sensing);
    public abstract Queue<Action> DecideTurn(int ap, int mp);
}
