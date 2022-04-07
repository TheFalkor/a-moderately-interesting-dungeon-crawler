using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupant : MonoBehaviour
{
    [SerializeField] protected EntityStatsSO stats;
    private int maxhealth;
    private int currentHealth;
    private int defense;

    private void Start()
    {
        maxhealth = stats.maxHealth;
        currentHealth = maxhealth;
        defense = stats.defense;
    }

}