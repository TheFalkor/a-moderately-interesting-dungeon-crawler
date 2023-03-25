using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] private GameObject OverworldParent;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject miniPlayer;

    private bool allowSelection = true;

    private AudioCore audioCore;

    [Header("Singleton")]
    public static OverworldManager instance;

    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void Start()
    {
        miniPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DungeonManager.instance.player.baseStat.miniSprite;

        audioCore = gameObject.GetComponent<AudioCore>();
    }

    public void SetVisible(bool active)
    {
        if (active)
            transitionAnimator.SetBool("Closed", false);
        OverworldParent.SetActive(active);
        allowSelection = active;
    }

    public void EnterDungeon(GameObject layout)
    {
        transitionAnimator.SetBool("Closed", true);
        StartCoroutine(DelayEnter(layout));
    }

    private IEnumerator DelayEnter(GameObject layout)
    {
        yield return new WaitForSeconds(1.25f);

        SetVisible(false);
        DungeonManager.instance.LoadDungeon(layout);
    }

    public void SetAllowSelection(bool active)
    {
        if (!active)
            allowSelection = false;
        else
        {
            StartCoroutine(DelayAllowSelection());
        }
    }

    private IEnumerator DelayAllowSelection()
    {
        yield return new WaitForSeconds(0.25f);
        allowSelection = true;
    }

    public bool GetAllowSelection()
    { return allowSelection; }

    public void MoveMiniPlayer(Transform newParent)
    {
        miniPlayer.transform.parent = newParent;
        Vector2 deltaPosition = new Vector2(miniPlayer.transform.position.x - newParent.position.x, 0);
        miniPlayer.transform.localPosition = new Vector3(0, -1f);

        if (deltaPosition.x != 0)
            miniPlayer.transform.localScale = new Vector2(deltaPosition.normalized.x, 1);
    }
}