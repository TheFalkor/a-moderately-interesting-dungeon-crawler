using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Combat Room", fileName = "Combat Room", order = 1)]
public class CombatRoomSO : ScriptableObject
{
    [Header("Information")]
    public string roomName;
    public string roomDescription;
    
    [Header("Room")]
    public TileData[] tiles = new TileData[70];
}
