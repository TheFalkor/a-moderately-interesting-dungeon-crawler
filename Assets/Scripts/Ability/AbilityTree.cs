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
    public int skillPoints = 0;

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
        abilityList.AddRange(player.baseStat.unlockableAbilities);
        abilityList.AddRange(player.classStat.unlockableAbilities);

        for (int i = 0; i < abilityList.Count; i++)
            unlockedAbilityList.Add(false);

        foreach (AbilitySO ability in player.baseStat.startingAbilities)
            UnlockAbility(ability);


        passiveList.AddRange(player.classStat.startingPassives);
        passiveList.AddRange(player.baseStat.unlockablePassives);
        passiveList.AddRange(player.classStat.unlockablePassives);

        for (int i = 0; i < passiveList.Count; i++)
            unlockedPassiveList.Add(false);

        foreach (PassiveSO passive in player.classStat.startingPassives)
            UnlockPassive(passive);
    }

    public void UnlockAbility(AbilitySO ability)
    {
        if (unlockedAbilityList[abilityList.IndexOf(ability)])
            return;

        unlockedAbilityList[abilityList.IndexOf(ability)] = true;

        AbilityManager.instance.AddAbility(CreateAbility(ability));
    }

    public void UnlockPassive(PassiveSO passive)
    {
        if (unlockedPassiveList[passiveList.IndexOf(passive)])
            return;

        unlockedPassiveList[passiveList.IndexOf(passive)] = true;

        PassiveManager.instance.AddPassive(CreatePassive(passive));
    }

    public bool IsAbilityUnlocked(AbilitySO ability)
    {
        return unlockedAbilityList[abilityList.IndexOf(ability)];
    }

    public bool IsPassiveUnlocked(PassiveSO passive)
    {
        return unlockedPassiveList[passiveList.IndexOf(passive)];
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
                ability = new ArcaneBlast();
                break;
            case AbilityID.IMBUE_WEAPON:
                ability = new ImbueWeapon();
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
                passive = new TimeRift();
                break;
            case PassiveID.TIME_BUBBLE:
                passive = new TimeBubble();
                break;
            case PassiveID.SPELL_BOOST:
                passive = new SpellBoost();
                break;
            case PassiveID.BLADE_DANCER:
                passive = new BladeDancer();
                break;
            case PassiveID.ARCANE_TRANSFERENCE:
                Debug.LogError("AbilityTree :: Forgot to create ability");
                break;
            case PassiveID.OVERLOAD:
                passive = new Overload();
                break;
            case PassiveID.CONDUIT_OF_POWER:
                passive = new ConduitOfPower();
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
                passive = new NecroticShroud();
                break;
            case PassiveID.MARKED_DECAY:
                passive = new MarkedDecay();
                break;
        }

        if (passive != null)
            passive.data = data;            

        return passive;
    }
}
