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
        foreach (AbilitySO ability in player.baseStat.abilities)
        {
            abilities[index] = CreateAbility(ability);
            index++;
        }

        foreach (AbilitySO ability in player.classStat.abilities)
        {
            abilities[index] = CreateAbility(ability);
            index++;
        }
    }

    public void SetupUI()
    {
        for (int i = 0; i < abilities.Length; i++)
            if (abilities[i] != null)
                CombatUI.instance.SetAbilityIcon(i, abilities[i].data.abilitySprite);
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

    private Ability CreateAbility(AbilitySO data)
    {
        Ability ability = null;

        switch (data.ability)
        {
            case AbilityID.FLUTTER_DASH:
                ability = new DashAbility();
                break;
            case AbilityID.TIME_PULSE:
                break;
            case AbilityID.ARCANE_BLAST:
                break;
            case AbilityID.IMBUE_WEAPON:
                break;
            case AbilityID.CORRUPTED_GROUNDS:
                ability = new CorruptedGroundsAbility();
                break;
            case AbilityID.SIPHON_DEATH:
                break;
        }

        if (ability != null)
            ability.data = data;

        return ability;
    }

}
