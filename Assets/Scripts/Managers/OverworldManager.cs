using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] private GameObject OverworldParent;

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
        audioCore = gameObject.GetComponent<AudioCore>();
    }

    public void SetVisible(bool active)
    {
        OverworldParent.SetActive(active);
    }
}
