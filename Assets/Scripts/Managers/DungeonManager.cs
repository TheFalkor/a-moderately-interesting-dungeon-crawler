using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private GameObject dungeonParent;
    [SerializeField] private Animator transitionAnimator;

    [Header("Runtime Variables")]
    private DungeonNode currentNode;
    private bool roomSelected = false;
    private float transitionTimer = 0;
    private bool start = false;
    

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
        if (!start)
        {
            ChangeMusic("MENU");
            start = true;
        }

        if (!roomSelected && Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if (hit.transform.GetComponent<DungeonNode>())
                {
                    if (!hit.transform.GetComponent<DungeonNode>().completed)
                    {
                        currentNode = hit.transform.GetComponent<DungeonNode>();
                        roomSelected = true;
                        transitionAnimator.SetBool("Closed", true);
                    }

                }
            }
        }

        if (roomSelected)
        {
            transitionTimer += Time.deltaTime;

            if (transitionTimer > 1.25f)
            {
                transitionTimer = 0;
                roomSelected = false;

                currentNode.EnterNode();
                ChangeMusic("COMBAT_2");
            }
        }
    }

    public void ToggleDungeonVisibility(bool active)
    {
        dungeonParent.SetActive(active);

    }

    private void ChangeMusic(string name)
    {
        gameObject.GetComponent<AudioKor>().PlayMusic(name, AudioKorLib.Enums.Track.A);
    }

    public void WonRoom()
    {
        currentNode.MarkCompleted();
    }
}
