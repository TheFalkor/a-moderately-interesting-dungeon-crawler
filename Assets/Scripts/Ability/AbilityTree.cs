using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTree : MonoBehaviour
{
    private readonly List<bool> unlockedAbilityList = new List<bool>();
    private readonly List<AbilitySO> abilityList = new List<AbilitySO>();

    private readonly List<bool> unlockedPassiveList = new List<bool>();
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

        abilityList.AddRange(player.baseStat.startingAbilities);
        abilityList.AddRange(player.classStat.startingAbilities);
        abilityList.AddRange(player.baseStat.unlockableAbilities);
        abilityList.AddRange(player.classStat.unlockableAbilities);

        for (int i = 0; i < abilityList.Count; i++)
            unlockedAbilityList.Add(false);

        foreach (AbilitySO ability in player.baseStat.startingAbilities)
            UnlockAbility(abilityList.IndexOf(ability));

        foreach (AbilitySO ability in player.classStat.startingAbilities)
            UnlockAbility(abilityList.IndexOf(ability));


        passiveList.AddRange(player.classStat.startingPassives);
        passiveList.AddRange(player.baseStat.unlockablePassives);
        passiveList.AddRange(player.classStat.unlockablePassives);

        for (int i = 0; i < passiveList.Count; i++)
            unlockedPassiveList.Add(false);

        foreach (PassiveSO passive in player.classStat.startingPassives)
            UnlockPassive(passiveList.IndexOf(passive));
    }

    public void UnlockAbility(int index)
    {
        if (unlockedAbilityList[index])
            return;

        unlockedAbilityList[index] = true;

        AbilityManager.instance.AddAbility(CreateAbility(abilityList[index]));
    }

    public void UnlockPassive(int index)
    {
        if (unlockedPassiveList[index])
            return;

        unlockedPassiveList[index] = true;

        PassiveManager.instance.AddPassive(CreatePassive(passiveList[index]));
    }

    private Ability CreateAbility(AbilitySO data)
    {
        Ability ability = null;

        switch (data.abilityType)
        {
            case AbilityID.FLUTTER_DASH:
                ability = new FlutterDash();
                break;
            case AbilityID.TIME_PULSE:
                ability = new TimePulse();
                break;
            case AbilityID.ARCANE_BLAST:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case AbilityID.IMBUE_WEAPON:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case AbilityID.CORRUPTED_GROUNDS:
                ability = new CorruptedGrounds();
                break;
            case AbilityID.SIPHON_DEATH:
                ability = new SiphonDeath();
                break;
        }

        if (ability != null)
            ability.data = data;

        return ability;
    }

    private Passive CreatePassive(PassiveSO data)
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
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.TIME_BUBBLE:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.SPELL_BOOST:
                passive = new SpellBoost();
                break;
            case PassiveID.BLADE_DANCER:
                passive = new BladeDancer();
                break;
            case PassiveID.ARCANE_BLAST:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.OVERLOAD:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.CONDUIT_OF_POWER:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.DEATH_MARK:
                passive = new DeathMark();
                break;
            case PassiveID.DEATH_SENTENCE:
                passive = new DeathSentence();
                break;
            case PassiveID.VITALITY_DRAIN:
                passive = new VitalityDrain();
                break;
            case PassiveID.NECROTIC_SHROUD:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.MARKED_DECAY:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
        }

        if (passive != null)
            passive.data = data;            

        return passive;
    }
}
