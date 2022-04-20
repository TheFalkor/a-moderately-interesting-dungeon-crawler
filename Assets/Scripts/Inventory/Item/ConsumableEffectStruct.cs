using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ConsumableEffectStruct 
{
    public ConsumableEffect myEffect;
    public int value;
    public ConsumableEffectStruct(ConsumableEffect effect,int effectValue) 
    {
        myEffect = effect;
        value = effectValue;
    }

}
