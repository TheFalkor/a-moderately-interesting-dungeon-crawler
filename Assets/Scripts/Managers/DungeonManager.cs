using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;


    [Header("Singleton")]
    public static DungeonManager instance;

    void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.transform.GetComponent<DungeonNode>())  // tmp
                    hit.transform.GetComponent<DungeonNode>().EnterNode();
            }
        }
    }

    public void ToggleDungeonVisibility(bool active)
    {
        dungeonParent.SetActive(active);
    }
}