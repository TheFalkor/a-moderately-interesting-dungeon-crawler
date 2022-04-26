using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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
}
