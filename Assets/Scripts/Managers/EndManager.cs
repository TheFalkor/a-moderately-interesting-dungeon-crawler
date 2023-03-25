using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [Header("End Screens")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject dungeonCompletedPopup;
    [SerializeField] private GameObject dungeonExitButton;
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

    public void SetExitDungeonPopup(bool active)
    {
        StartCoroutine(DelayExitDungeonPopup(active));
    }

    private IEnumerator DelayExitDungeonPopup(bool active)
    {
        yield return new WaitForSeconds(1.60f);
        dungeonCompletedPopup.SetActive(active);
        DungeonManager.instance.allowSelection = false;
    }

    public void OpenExitDungeonPopup()
    {
        dungeonCompletedPopup.SetActive(true);
        DungeonManager.instance.allowSelection = false;
    }

    public void CloseExitDungeonPopup()
    { 
        dungeonCompletedPopup.SetActive(false);

        if (dungeonExitButton.activeSelf == false)
            dungeonExitButton.SetActive(true);

        DungeonManager.instance.allowSelection = true;
    }

    public void CloseDungeon()
    {
        StartCoroutine(DelayCloseDungeon());
        dungeonCompletedPopup.SetActive(false);
    }

    private IEnumerator DelayCloseDungeon()
    {
        yield return new WaitForSeconds(1.25f);

        dungeonExitButton.SetActive(false);
        dungeonCompletedPopup.SetActive(false);
    }

    public void EndGame(bool win)
    {
        gameEnd = true;
        this.win = win;

        CombatManager.instance.StopGame();
    }

    private void End()
    {
        if (win)
            winScreen.SetActive(true);
        else
        {
            gameObject.GetComponent<AudioCore>().PauseMusic();
            gameObject.GetComponent<AudioCore>().PlaySFX("YOU_DIED");
            loseScreen.SetActive(true);
        }
    }

    // TODO: FIX NEW GAME PLUS (Dungeon Layout not updated for this)
    public void NewGamePlus()
    {
        gameEnd = false;
        win = false;
        DungeonManager.instance.allowSelection = false;

        ConsistentData.difficultyScale += 0.25f;
        DungeonManager.instance.ResetGame();
        DungeonManager.instance.InitializeLayout();

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
