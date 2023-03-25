using AudioCoreLib.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopBarManager : MonoBehaviour
{
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject exitParent;

    private AudioCore audioCore;

    [Header("Singleton")]
    public static TopBarManager instance;

    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    private void Start()
    {
        audioCore = gameObject.GetComponent<AudioCore>();
    }

    public void SetVisible(bool active)
    {
        topBar.SetActive(active);
    }

    public void OpenInventory(int tab)
    {
        DungeonManager.instance.allowSelection = false;
        OverworldManager.instance.SetAllowSelection(false);
        InventoryUI.instance.ShowUI(tab);
    }

    public void OpenExitPopup()
    {
        if (DungeonManager.instance.allowSelection)
            DungeonManager.instance.allowSelection = false;

        OverworldManager.instance.SetAllowSelection(false);

        audioCore.PlaySFX("SELECT");
        exitParent.SetActive(true);
    }

    public void ExitPopupChoice(bool endGame)
    {
        if (endGame)
            SceneManager.LoadScene(0);
        else
            exitParent.SetActive(false);

        DungeonManager.instance.RemoveRestrictions();
        OverworldManager.instance.SetAllowSelection(true);
    }
}
