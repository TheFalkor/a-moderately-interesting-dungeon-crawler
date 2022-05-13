using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [Header("End Screens")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;


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
        gameEnd = true;
        this.win = win;
    }

    private void End()
    {
        if (win)
            winScreen.SetActive(win);
        else if (!win)
            loseScreen.SetActive(!win);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
