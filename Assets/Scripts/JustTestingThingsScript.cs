using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JustTestingThingsScript : MonoBehaviour
{
   // remember to test health potion(Potato)
    SkillTree tree = new SkillTree();
    public ScriptableItem healtpotion;
    Inventory myInventory = new Inventory();
    public Occupant player;
    // Start is called before the first frame update
    void Start()
    {
        myInventory.SetOwner(player);
        myInventory.AddItem(healtpotion.CreateItem());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            tree.UnlockSkill(SkillId.DASH);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            tree.RefundSkill(SkillId.DASH);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            tree.UnlockSkill(SkillId.TIME_BUBBLE);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4)) 
        {
            tree.RefundSkill(SkillId.TIME_BUBBLE);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            myInventory.UseItem(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            tree.TakeSkillPoints(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            tree.GiveSkillPoints(1);
        }
        

        if (Input.GetKeyUp(KeyCode.Alpha0)) 
        {
            for(int i = 0; i < SkillTree.numberOfSkills; i++) 
            {
                SkillId currentSkill = (SkillId)i;
                Debug.Log(currentSkill.ToString() + ":" + tree.HasSkill(currentSkill));
            }
            Debug.Log("Skillpoints left:" + tree.GetSkillPoints());
        }
    }
}
