using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private GameObject selectionMarker;

    private List<InventorySlot> hotbarSlots = new List<InventorySlot>();
    private List<int> hotbarSlotIndex = new List<int>();

    private int selectedHotbarIndex = -1;
    private Inventory inventory;
    private Player player;

    [Header("Singleton")]
    public static HotbarUI instance;


    private void Awake()
    {
        if (instance)
            return;

        instance = this;
    }

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<Inventory>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        for (int i = 1; i < transform.childCount; i++)
        {
            Image image = transform.GetChild(i).GetChild(0).GetComponent<Image>();
            Text text = transform.GetChild(i).GetChild(1).GetComponent<Text>();

            hotbarSlots.Add(new InventorySlot(image, text, selectionMarker));

            hotbarSlotIndex.Add(-1);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (InventorySlot slot in hotbarSlots)
            slot.ClearSlot();

        for (int i = 0; i < hotbarSlotIndex.Count; i++)
            hotbarSlotIndex[i] = -1;

        int slotIndex = 0;
        for (int i = 0; i < inventory.items.Length; i++)
        {
            Item item = inventory.items[i];

            if (item == null)
                continue;

            if (item.itemType == ItemType.CONSUMABLE || item.itemType == ItemType.THROWABLE)
            {
                hotbarSlots[slotIndex].SetSlot(item);
                hotbarSlotIndex[slotIndex] = i;

                hotbarSlots[slotIndex].SetSlotActive(true);

                slotIndex++;
            }
        }

        selectionMarker.SetActive(false);
    }

    public void EnableHotbar()
    {
        foreach (InventorySlot slot in hotbarSlots)
        {
            if (slot.image.gameObject.activeSelf)
                slot.SetSlotActive(true);
        }
    }

    public void SelectItem(int index)
    {
        if (index != -1)
            CombatUI.instance.SelectAbility(-1);

        if (index == -1 || hotbarSlotIndex[index] == -1 || index == selectedHotbarIndex)
        {
            selectedHotbarIndex = -1;
            player.SelectItem(-1);
            //CombatUI.instance.SetAttackButton(false);
            selectionMarker.SetActive(false);
            return;
        }

        hotbarSlots[index].SelectSlot();
        selectedHotbarIndex = index;

        CombatUI.instance.SetAttackButton(true);
        player.SelectItem(hotbarSlotIndex[index]);
    }

    public void DisableItemUI()
    {
        selectionMarker.gameObject.SetActive(false);

        foreach (InventorySlot slot in hotbarSlots)
            slot.SetSlotActive(false);
    }
}
