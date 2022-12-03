using System.Collections.Generic;
using UnityEngine;

public class BaseRoomSO : ScriptableObject
{
    [Header("Information")]
    [HideInInspector] public RoomType roomType;
    public string roomName;
    public string roomDescription;

    [Header("Reward")]
    public List<ItemSO> rewards;
}
