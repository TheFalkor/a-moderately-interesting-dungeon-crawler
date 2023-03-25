using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    //To keep track of buttons to select when changing canvas when not using mouse
    [Header("Standard Selected Buttons")]
    [SerializeField] private Animator transitionAnimator;
    private GameObject lastSelected;
    [Space]
    private AudioCore audioCore;
    private bool isMusicOn;

    private void Update()
    {
        if(!isMusicOn)
        {
            audioCore = gameObject.GetComponent<AudioCore>();
            audioCore.PlayMusic("MENU", AudioCore.Track.A);
            isMusicOn = true;
        }
    }

    public void StartGame()
    {
        SelectionController selection = GetComponent<SelectionController>();
        ConsistentData.SetBaseStat(selection.GetBaseStat());
        ConsistentData.SetClassStat(selection.GetClassStat());

        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        audioCore.PauseMusic();
        audioCore.PlaySFX("START_GAME");
        transitionAnimator.SetBool("Closed", true);

        yield return new WaitForSeconds(1.05f);

        SceneManager.LoadScene("MarioOverworldTest");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //Saves selected button when changing canvas
    public void ChangeCanvas(GameObject newSelect)
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
        SelectChange(newSelect);
    }

    public void ReturnToMain()
    {
        SelectChange(lastSelected);
    }

    private void SelectChange(GameObject newSelect)
    {
        //Reset needs to happen before selecting new object
        EventSystem.current.SetSelectedGameObject(null);

        //Selects new Object
        EventSystem.current.SetSelectedGameObject(newSelect);

        OnClick();
    }

    public void OnClick()
    {
        if (audioCore == null)
            return;
        audioCore.PlaySFX("SELECT");
    }
}
