using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEquipmentBar : SetUpDungeonInventory
{
    public float gapSizeMultiplier = 0;
    public SetUpDungeonInventory parent;
    protected override void CreateButton(int index)
    {
        return;
        //int widthInButtons = (int)(containerWidth / buttonWidth);
        int widthInButtons = 1;
        if (widthInButtons < 1)
        {
            widthInButtons = 1;
        }
        float xPos = -(containerWidth / 2 - buttonWidth / 2 - buttonWidth * (index % widthInButtons));
        
        float yPos =containerHeight / 2 - (buttonHeight*(1+gapSizeMultiplier)) / 2 - (buttonHeight*(1+gapSizeMultiplier)) * Mathf.Floor((index / widthInButtons));
        CreateButtonAtPos(xPos, yPos, index);
    }

    public override void UpdateSprites()
    {
        for (int i = 0; i < buttonImages.Count; i++)
        {
            EquipmentType equipment = EquipmentType.UNASIGNED;
            int slotNr = 0;
            switch (i) 
            {
                case (int)(EquipmentType.WEAPON):equipment = EquipmentType.WEAPON;break;
                case (int)(EquipmentType.ARMOR): equipment = EquipmentType.ARMOR; break;
                case (int)(EquipmentType.ACCESSORY): equipment = EquipmentType.ACCESSORY;break;
                case (int)(EquipmentType.ACCESSORY)+1: equipment = EquipmentType.ACCESSORY;slotNr = 1; break;
                default : equipment = EquipmentType.UNASIGNED; break;
            }
            Sprite spr = null;
            if (spr == null)
            {
                spr = defaultSprite;
            }
            buttonImages[i].sprite = spr;
        }
    }

    protected override void WireUpButton(Button button, int index)
    {
        EquipmentType equipment = EquipmentType.UNASIGNED;
        int slotNr = 0;
        switch (index)
        {
            case (int)(EquipmentType.WEAPON): equipment = EquipmentType.WEAPON; break;
            case (int)(EquipmentType.ARMOR): equipment = EquipmentType.ARMOR; break;
            case (int)(EquipmentType.ACCESSORY): equipment = EquipmentType.ACCESSORY; break;
            case (int)(EquipmentType.ACCESSORY) + 1: equipment = EquipmentType.ACCESSORY; slotNr = 1; break;
            default: equipment = EquipmentType.UNASIGNED; break;
        }

        button.onClick.AddListener(delegate { playerScript.UnequipItem(equipment, slotNr); });
        if (parent != null) 
        {
            button.onClick.AddListener(delegate { parent.UpdateSprites(); });
        }
        
    }
}
