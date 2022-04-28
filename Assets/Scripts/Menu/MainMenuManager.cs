using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    //To keep track of buttons to select when changing canvas when not using mouse
    [Header("Standard Selected Buttons")]
    private GameObject lastSelected;
    private bool isMusicOn = false;//Just test

    private void Update()
    {
        if (!isMusicOn)
        {
            gameObject.GetComponent<AudioKor>().PlayMusic("MENU");
            isMusicOn = true;
        }
    }

    public void StartGame()
    {
        SelectionController selection = GetComponent<SelectionController>();
        ConsistentData.SetBaseStat(selection.GetBaseStat());
        ConsistentData.SetClassStat(selection.GetClassStat());

        SceneManager.LoadScene("main");
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
    }
}
