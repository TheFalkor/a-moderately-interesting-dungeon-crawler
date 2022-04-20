using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JustTestingThingsScript : MonoBehaviour
{
   
    SkillTree tree = new SkillTree();
    // Start is called before the first frame update
    void Start()
    {
       
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
