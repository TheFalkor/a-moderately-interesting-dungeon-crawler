using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "ScriptableObjects/Item/Effects/ConsumableEffect")]
public class ScriptableConsumableEffect : ScriptableObject
{
    public ConsumableEffect effect;
    public int value;
    public ConsumableEffectStruct CreateEffect() 
    {
        return new ConsumableEffectStruct(effect, value);
    }
}
