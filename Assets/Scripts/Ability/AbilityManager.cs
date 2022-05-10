using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [Header("Runtime Variables")]
    private Ability[] abilities = new Ability[4];

    private Player player;

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
        player = DungeonManager.instance.player;

        int index = 0;
        foreach (AbilitySO ability in player.baseStat.startingAbilities)
        {
            abilities[index] = AbilityTree.instance.CreateAbility(ability);
            index++;
        }

        foreach (AbilitySO ability in player.classStat.startingAbilities)
        {
            abilities[index] = AbilityTree.instance.CreateAbility(ability);
            index++;
        }
    }

    public void SetupUI()
    {
        for (int i = 0; i < abilities.Length; i++)
            if (abilities[i] != null)
            {
                CombatUI.instance.SetAbilityIcon(i, abilities[i].data);
            }
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
