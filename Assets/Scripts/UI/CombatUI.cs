using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject statsBox;
    [SerializeField] private Transform abilityBox;
    private Text healthText;
    private Text attackText;
    private Text defenseText;
    private Text actionPointText;
    [Space]
    private readonly List<Image> abilityIcons = new List<Image>();
    [Space]
    [SerializeField] private Text weaponButtonText;
    [Space]
    [SerializeField] private Image playerPortrait;


    [Header("Game Variables")]
    private bool attackMode = false;
    private int selectedAbilityIndex = -1;


    [Header("GameObject References")]
    private Player player;


    [Header("Singleton")]
    public static CombatUI instance;

    
    private void Awake()
    {
        if (instance)
            return;

        instance = this;

        healthText = statsBox.transform.Find("Health Text").GetComponent<Text>();
        attackText = statsBox.transform.Find("Attack Text").GetComponent<Text>();
        defenseText = statsBox.transform.Find("Defense Text").GetComponent<Text>();
        actionPointText = statsBox.transform.Find("Action Points Temp").GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int i = 1; i < abilityBox.childCount; i++)
        {
            abilityIcons.Add(abilityBox.GetChild(i).GetChild(0).GetComponent<Image>());
        }

        if (ConsistentData.initialized)
            SetPortrait(ConsistentData.playerBaseStat.entitySprite);

        AbilityManager.instance.SetupUI();
    }

    public void ToggleAttackMode()
    {
        SetAttackButton(!attackMode);
     
        player.SetAttackMode(attackMode);
    }

    public void SelectAbility(int index)
    {
        Ability ability = AbilityManager.instance.GetAbility(index);

        if (ability != null && index != selectedAbilityIndex)
            SetAttackButton(true);
        else
            SetAttackButton(false);

        if (attackMode)
        {
            selectedAbilityIndex = index;

            if (!player.abilityActive)
                GridManager.instance.ClearAllHighlights();

            if (!player.SelectAbility(ability))
                SelectAbility(index);
        }
        else
            player.SelectAbility(null);

        if (index != -1)
            HotbarUI.instance.SelectItem(-1);
    }

    public void EndPlayerTurn()
    {
        player.EndTurn();
    }

    public void SetAttackButton(bool active)
    {
        attackMode = active;

        if (active)
        {
            weaponButtonText.text = "MOVE AGANE";
        }
        else
        {
            weaponButtonText.text = "USE WEAPON";
            selectedAbilityIndex = -1;
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth, int shield)
    {
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;

        if (shield != 0)
            healthText.text = healthText.text + " (+" + shield + ")";
    }

    public void UpdateAttack(int attackDamage)
    {
        attackText.text = "ATK: " + attackDamage.ToString();
    }
    
    public void UpdateDefense(int defense)
    {
        defenseText.text = "DEF: " + defense.ToString();
    }

    public void UpdateActionPoints(int movementPoints, int actionPoints)
    {
        actionPointText.text = "AP: " + actionPoints + "   MP: " + movementPoints;
    }

    public void SetAbilityIcon(int index, AbilitySO data)
    {
        abilityIcons[index].gameObject.SetActive(true);
        abilityIcons[index].sprite = data.abilitySprite;

        abilityIcons[index].transform.parent.GetComponent<Hoverable>().SetInformation(data.abilityName, data.abilitySummary, data.abilityDescription);
    }

    public void SetPortrait(Sprite sprite)
    {
        playerPortrait.sprite = sprite;
    }

    public void RotatePortrait(RectTransform body)
    {
        body.eulerAngles += new Vector3(0, 0, -90);
    }
}
