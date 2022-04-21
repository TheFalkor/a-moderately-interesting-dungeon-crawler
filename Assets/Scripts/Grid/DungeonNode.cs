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

        CombatManager.instance.StartCombat(room);
        DungeonManager.instance.ToggleDungeonVisibility(false);

        //MarkCompleted();    // tmp
    }

    public void MarkCompleted()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
