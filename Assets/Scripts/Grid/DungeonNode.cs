using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNode : MonoBehaviour
{
    [Header("Dungeon Setup")]
    public CombatRoomSO room;


    [Header("Runtime Variables")]
    [HideInInspector] public bool completed = true;


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

    public bool EnterNode()
    {
        if (completed)
            return false;

        CombatManager.instance.StartCombat(room);
        DungeonManager.instance.ToggleDungeonVisibility(false);

        return true;
        //MarkCompleted();    // tmp
    }

    public void MarkCompleted()
    {
        completed = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
