using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JustTestingThingsScript : MonoBehaviour// script is just for testing and is not suposed to be implimented.
{
   
    
    public List<ScriptableItem> itemsToAdd;
    Inventory myInventory = new Inventory();
    public Occupant player;
    int nr=0;
    // Start is called before the first frame update
    void Start()
    {
        myInventory.SetOwner(player);
        myInventory.CreateEquipmentInventory();
        foreach (ScriptableItem s in itemsToAdd) 
        {
            myInventory.AddItem(s.CreateItem());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
           myInventory.UseItem(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            myInventory.UseItem(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            myInventory.UseItem(2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4)) 
        {
            myInventory.UseItem(3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            myInventory.UseItem(4);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            myInventory.UseItem(5);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            myInventory.UseItem(6);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            
        }
        

        if (Input.GetKeyUp(KeyCode.Alpha0)) 
        {
            
            for (int i=0;i<myInventory.GetAmountOfItems();i++) 
            {
                Debug.Log("test:"+nr+" Slot " + i + ":" + myInventory.GetItem(i).GetName());
            }
            Debug.Log("test:" + nr + " attack:"+myInventory.EquipedItemsStatValue(StatType.ATTACK)+
                " defense:"+myInventory.EquipedItemsStatValue(StatType.DEFENSE)+" max health:"+
                myInventory.EquipedItemsStatValue(StatType.MAX_HEALTH)); 
            nr++;
        }
    }
}
