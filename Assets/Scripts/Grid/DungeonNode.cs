using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode : MonoBehaviour
{
    [Header("Dungeon Setup")]
    public CombatRoomSO room;


    [Header("Runtime Variables")]
    private bool completed = true;


    void Start()
    {
        if (room)
        {
            completed = false;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void EnterNode()
    {
        if (completed)
            return;
        Debug.Log("START DUNGEON: " + room.roomName);
        // CombatManager.StartCombat();

        MarkCompleted();    // tmp
    }

    public void MarkCompleted()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
