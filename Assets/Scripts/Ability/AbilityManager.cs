using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Runtime Variables")]
    private Ability[] abilities = new Ability[4];

    public GameObject tempPoolprefab;

    [Header("Singleton")]
    public static AbilityManager instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Start()
    {
        abilities[0] = new DashAbility();
        abilities[1] = new CorruptedGroundsAbility(tempPoolprefab);
    }

    public Ability GetAbility(int index)
    {
        if (index < 0 || index > 3)
            return null;

        return abilities[index];
    }

    public TileEffect SpawnTileEffect(GameObject effectPrefab)
    {
        return Instantiate(effectPrefab, new Vector2(0, 0), Quaternion.identity).GetComponent<TileEffect>();
    }

}
