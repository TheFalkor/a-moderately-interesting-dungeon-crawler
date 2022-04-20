using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject statsBox;
    private Text healthText;
    private Text attackText;
    private Text defenseText;
    [Space]
    [SerializeField] private Text weaponButtonText;


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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void ToggleAttackMode()
    {
        attackMode = !attackMode;
        UpdateAttacKButton();
     
        player.SetAttackMode(attackMode);
    }

    public void SelectAbility(int index)
    {
        if (index != selectedAbilityIndex)
            attackMode = true;
        else
            attackMode = !attackMode;

        UpdateAttacKButton();

        selectedAbilityIndex = index;
        // Select
    }

    private void UpdateAttacKButton()
    {
        if (attackMode)
        {
            weaponButtonText.text = "MOVE AGANE";
        }
        else
        {
            weaponButtonText.text = "USE WEAPON";
            selectedAbilityIndex = -1;
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
    }

    public void UpdateAttack(int attackDamage)
    {
        attackText.text = "ATK: " + attackDamage.ToString();
    }
    
    public void UpdateDefense(int defense)
    {
        defenseText.text = "DEF: " + defense.ToString();
    }



    public void RotatePortrait(RectTransform transform)
    {
        transform.eulerAngles += new Vector3(0, 0, 90);
    }
}
