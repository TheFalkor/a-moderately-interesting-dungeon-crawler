using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Scriptable Objects/Room/Treasure", fileName = "Treasure Room", order = 2)]
public class TreasureRoomSO : BaseRoomSO
{

    public void Reset()
    {
        roomType = RoomType.TREASURE;
    }
}
