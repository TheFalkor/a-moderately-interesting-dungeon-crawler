using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject statsBox;
    [SerializeField] private Transform abilityBox;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField] private Animator endTurnAnimator;
    [Space]
    [SerializeField] private GameObject selectionMarker;
    private Text healthText;
    private Text attackText;
    private Text defenseText;
    private Text actionPointText;
    private Text movementPointText;
    [Space]
    private readonly List<AbilitySlot> abilitySlots = new List<AbilitySlot>();
    [Space]
    [SerializeField] private Text weaponButtonText;
    [Space]
    [SerializeField] private Image combatPortrait;
    [SerializeField] private Image inventoryPortrait;
    [SerializeField] private Image victoryPortrait;
    [SerializeField] private Image dungeonPortrait;
    [Space]
    [SerializeField] private Transform equipmentHoverParent;
    private Hoverable[] equipmentHovers = new Hoverable[3];


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

        healthText = statsBox.transform.GetChild(2).GetChild(0).GetComponent<Text>();
        attackText = statsBox.transform.GetChild(3).GetChild(0).GetComponent<Text>();
        defenseText = statsBox.transform.GetChild(4).GetChild(0).GetComponent<Text>();
        actionPointText = statsBox.transform.GetChild(5).GetChild(0).GetComponent<Text>();
        movementPointText = statsBox.transform.GetChild(6).GetChild(0).GetComponent<Text>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int i = 1; i < abilityBox.childCount; i++)
        {
            abilitySlots.Add(new AbilitySlot(abilityBox.GetChild(i).GetChild(0).GetComponent<Image>(), selectionMarker));
        }

        if (ConsistentData.initialized)
            SetPortrait(ConsistentData.playerBaseStat.entitySprite);

        for (int i = 0; i < 3; i++)
        {
            equipmentHovers[i] = equipmentHoverParent.GetChild(i).GetComponent<Hoverable>();
            equipmentHovers[i].gameObject.SetActive(false);
        }

        AbilityManager.instance.SetupUI();
        InventoryUI.instance.UpdateEquippedUI();
    }

    public void ToggleAttackMode()
    {
        SetAttackButton(!attackMode);
     
        player.SetAttackMode(attackMode);
    }

    public void SelectAbility(int index)
    {
        selectionMarker.SetActive(false);
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
            else
                abilitySlots[index].SelectSlot();
        }
        else
            player.SelectAbility(null);

        if (index != -1)
            HotbarUI.instance.SelectItem(-1);
    }

    public void EndPlayerTurn()
    {
        player.EndTurn();
        endTurnAnimator.SetBool("Glow", false);
    }

    public void EnableEndTurnGlow()
    {
        endTurnAnimator.SetBool("Glow", true);
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
        healthText.text = currentHealth + "/" + maxHealth;

        if (shield != 0)
            healthText.text = healthText.text + " (+" + shield + ")";
    }

    public void UpdateAttack(int attackDamage)
    {
        attackText.text = attackDamage.ToString();
    }
    
    public void UpdateDefense(int defense)
    {
        defenseText.text = defense.ToString();
    }

    public void UpdateActionPoints(int movementPoints, int actionPoints)
    {
        actionPointText.text = actionPoints.ToString();
        movementPointText.text = movementPoints.ToString();

        UpdateAbilityUI();
    }

    public void UpdateAbilityUI()
    {
        int playerAP = player.GetCurrentAP();

        for (int i = 0; i < 4; i++)
        {
            Ability ability = AbilityManager.instance.GetAbility(i);

            if (ability == null)
                continue;

            if (ability.cooldown > 0)
            {
                abilitySlots[i].image.transform.parent.GetChild(1).GetComponent<Text>().text = ability.cooldown.ToString();
                abilitySlots[i].SetSlotActive(false);
            }
            else
            {
                abilitySlots[i].image.transform.parent.GetChild(1).GetComponent<Text>().text = "";
                if (playerAP >= ability.data.actionPointCost)
                    abilitySlots[i].SetSlotActive(true);
                else
                    abilitySlots[i].SetSlotActive(false);
            }
        }
    }

    public void DisableAbilityUI()
    {
        foreach (AbilitySlot slot in abilitySlots)
            slot.SetSlotActive(false);
    }

    public void SetAbilityIcon(int index, AbilitySO data)
    {
        if (data != null)
        {
            abilitySlots[index].SetSlot(data);
            abilitySlots[index].image.transform.parent.GetComponent<Hoverable>().SetInformation(data.abilityName, data.abilitySummary, data.abilityDescription);
        }
        else
        {
            abilitySlots[index].SetSlotActive(false);
        }
    }

    public void SetEquipmentData(EquippableItem item, int index)
    {
        equipmentHovers[index].transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        equipmentHovers[index].SetInformation(item.itemName, item.itemSummary, item.itemDescription);

        equipmentHovers[index].gameObject.SetActive(true);
    }

    public void SetPortrait(Sprite sprite)
    {
        combatPortrait.sprite = sprite;
    }

    public void RotatePortrait()
    {
        combatPortrait.rectTransform.eulerAngles += new Vector3(0, 0, -90);
        inventoryPortrait.rectTransform.eulerAngles += new Vector3(0, 0, -90);
        victoryPortrait.rectTransform.eulerAngles += new Vector3(0, 0, -90);
        dungeonPortrait.rectTransform.eulerAngles += new Vector3(0, 0, -90);

        // Cat Rave Feature
        if (combatPortrait.rectTransform.eulerAngles.z == 180)
            backgroundImage.SetActive(true);
        else if (combatPortrait.rectTransform.eulerAngles.z == 0)
            backgroundImage.SetActive(false);
    }
}
