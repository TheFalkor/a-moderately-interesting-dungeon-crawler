using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Scriptable Objects/Combat Room", fileName = "Combat Room", order = 1)]
public class CombatRoomSO : ScriptableObject
{
    [Header("Information")]
    public string roomName;
    public string roomDescription;

    [Header("Room")]
    public List<ItemSO> rewards;
    public TileData[] tiles = new TileData[70];
}
