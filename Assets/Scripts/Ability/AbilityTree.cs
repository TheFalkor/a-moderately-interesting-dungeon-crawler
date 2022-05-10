using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTree : MonoBehaviour
{
    private readonly List<AbilitySO> unlockedAbilityList = new List<AbilitySO>();
    private readonly List<AbilitySO> abilityList = new List<AbilitySO>();
    private readonly List<PassiveSO> unlockedPassiveList = new List<PassiveSO>();
    private readonly List<PassiveSO> passiveList = new List<PassiveSO>();

    private Player player;

    [Header("Singleton")]
    public static AbilityTree instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Start()
    {
        player = DungeonManager.instance.player;

        unlockedAbilityList.AddRange(player.baseStat.startingAbilities);
        abilityList.AddRange(player.baseStat.unlockableAbilities);
        abilityList.AddRange(player.classStat.unlockableAbilities);

        unlockedPassiveList.AddRange(player.classStat.startingPassives);
        passiveList.AddRange(player.baseStat.unlockablePassives);
        passiveList.AddRange(player.classStat.unlockablePassives);
    }

    public Ability CreateAbility(AbilitySO data)
    {
        Ability ability = null;

        switch (data.abilityType)
        {
            case AbilityID.FLUTTER_DASH:
                ability = new FlutterDash();
                break;
            case AbilityID.TIME_PULSE:
                break;
            case AbilityID.ARCANE_BLAST:
                break;
            case AbilityID.IMBUE_WEAPON:
                break;
            case AbilityID.CORRUPTED_GROUNDS:
                ability = new CorruptedGrounds();
                break;
            case AbilityID.SIPHON_DEATH:
                break;
        }

        if (ability != null)
            ability.data = data;

        return ability;
    }

    public Passive CreatePassive(PassiveSO data)
    {
        Passive passive = null;

        switch (data.passiveType)
        {
            case PassiveID.FLUTTER_SPREAD:
                passive = new FlutterSpread();
                break;
            case PassiveID.THORN_DASH:
                passive = new ThornDash();
                break;
            case PassiveID.TIME_RIFT:
                break;
            case PassiveID.TIME_BUBBLE:
                break;
            case PassiveID.SPELL_BOOST:
                break;
            case PassiveID.BLADE_DANCER:
                break;
            case PassiveID.ARCANE_BLAST:
                break;
            case PassiveID.OVERLOAD:
                break;
            case PassiveID.CONDUIT_OF_POWER:
                break;
            case PassiveID.DEATH_MARK:
                break;
            case PassiveID.DEATH_SENTENCE:
                break;
            case PassiveID.VITALITY_DRAIN:
                break;
            case PassiveID.NECROTIC_SHROUD:
                break;
            case PassiveID.MARKED_DECAY:
                break;
        }

        return passive;
    }
}
