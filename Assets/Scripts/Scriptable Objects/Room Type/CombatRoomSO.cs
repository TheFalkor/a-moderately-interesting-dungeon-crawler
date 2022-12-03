using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Scriptable Objects/Room/Combat", fileName = "Combat Room", order = 1)]
public class CombatRoomSO : BaseRoomSO
{
    [Header("Combat")]
    public TileData[] tiles = new TileData[70];

    public void Reset()
    {
        roomType = RoomType.COMBAT;
    }
}
