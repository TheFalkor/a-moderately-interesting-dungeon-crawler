using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private GameObject victoryPopup;

    private float timer = 0;
    private bool count = false;

    [Header("Singleton")]
    public static VictoryManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (count)
        {
            timer += Time.deltaTime;

            if (timer > 1.25f)
            {
                count = false;
                victoryPopup.SetActive(false);
                DungeonManager.instance.ToggleDungeonVisibility(true);
                CombatManager.instance.HideRoom();
                transitionAnimator.SetBool("Closed", false);
            }
        }
    }

    public void ShowPopup()
    {
        victoryPopup.SetActive(true);
    }

    public void ReturnDungeon()
    {
        count = true;
        timer = 0;
        DungeonManager.instance.WonRoom();
        transitionAnimator.SetBool("Closed", true);
    }
}
