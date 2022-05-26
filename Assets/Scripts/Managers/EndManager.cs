using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [Header("End Screens")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private Animator transitionAnimator;


    [Header("Runtime Variables")]
    private float transitionTimer = 0;
    private bool gameEnd = false;
    private bool win = false;

    [Header("Singleton")]
    public static EndManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void Update()
    {
        if(gameEnd)
        {
            transitionTimer += Time.deltaTime;
            if (transitionTimer > 1.25f)
            {
                gameEnd = false;
                transitionTimer = 0;
                End();
            }
        }
    }

    public void EndGame(bool win)
    {
        if (win)
            gameEnd = true;
        this.win = win;
        CombatManager.instance.StopGame();

        if (!win)
            End();
    }

    private void End()
    {
        if (win)
            winScreen.SetActive(win);
        else if (!win)
            loseScreen.SetActive(!win);
    }

    public void NewGamePlus()
    {
        gameEnd = false;
        win = false;
        DungeonManager.instance.allowSelection = false;

        ConsistentData.difficultyScale += 0.25f;
        DungeonManager.instance.ResetGame();

        DungeonNode startNode = DungeonManager.instance.allNodes[0];

        foreach (DungeonNode node in DungeonManager.instance.allNodes)
            node.GGC_NewGamePlus();

        DungeonManager.instance.allNodes.Clear();
        startNode.Initialize(startNode.room, startNode);

        StartCoroutine(Transition());
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator Transition()
    {
        transitionAnimator.SetBool("Closed", true);

        yield return new WaitForSeconds(1.25f);
        DungeonManager.instance.allowSelection = true;

        winScreen.SetActive(false);
        transitionAnimator.SetBool("Closed", false);
    }
}
