using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform combatSlotParent;
    private List<InventorySlot> combatSlots = new List<InventorySlot>();

    private Inventory inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();

        for (int i = 1; i < combatSlotParent.childCount; i++)
        {
            Image image = combatSlotParent.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = combatSlotParent.GetChild(i).GetChild(0).GetComponent<Text>();

            combatSlots.Add(new InventorySlot(image, text));
        }
    }

    void Update()
    {
        
    }
}
