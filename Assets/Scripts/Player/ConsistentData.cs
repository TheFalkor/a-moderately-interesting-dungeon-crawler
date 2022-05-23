using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsistentData : MonoBehaviour
{
    public static bool initialized = false;
    public static BaseStatsSO playerBaseStat;
    public static ClassStatsSO playerClassStat;
    public static float difficultyScale = 1.0f;

    void Start()
    {
        if (playerBaseStat)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public static void SetBaseStat(BaseStatsSO baseStat)
    {
        initialized = true;
        playerBaseStat = baseStat;
        difficultyScale = 1;
    }

    public static void SetClassStat(ClassStatsSO classStat)
    {
        initialized = true;
        playerClassStat = classStat;
        difficultyScale = 1;
    }
}
