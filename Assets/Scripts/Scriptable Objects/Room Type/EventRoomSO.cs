using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Scriptable Objects/Room/Event", fileName = "Event Room", order = 2)]
public class EventRoomSO : BaseRoomSO
{
    [Header("Event")]
    public string EventTitle;
    public string EventDescription;

    public void Reset()
    {
        roomType = RoomType.EVENT;
    }
}
