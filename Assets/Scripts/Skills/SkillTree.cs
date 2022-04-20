using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree
{
    public const int numberOfSkills = (int)(SkillId.count);
    bool[] mySkills = new bool[numberOfSkills];
    bool unlimitedMode = false;
    int skillpoints;
    public SkillTree()
    {
        skillpoints = 0;
        for (int i = 0; i < numberOfSkills; i++)
        {
            mySkills[i] = false;
        }
    }

    public void GiveSkillPoints(int amount)
    {
        skillpoints += amount;
    }
    public void TakeSkillPoints(int amount)
    {
        skillpoints -= amount;
        if (skillpoints < 0)
        {
            skillpoints = 0;
        }
    }
    public bool UnlockSkill(SkillId skill)
    {
        int skillIndex = (int)(skill);
        if (skillIndex < numberOfSkills && !mySkills[skillIndex])
        {
            if (skillpoints > 0 || unlimitedMode)
            {
                if (!unlimitedMode)
                {
                    skillpoints--;
                }
                mySkills[skillIndex] = true;
                return true;
            }

        }
        return false;
    }

    public bool RefundSkill(SkillId skill)
    {
        int skillIndex = (int)(skill);
        if (skillIndex < numberOfSkills && mySkills[skillIndex])
        {
            if (!unlimitedMode)
            {
                skillpoints++;
            }
            mySkills[skillIndex] = false;
            return true;
        }
        return false;
    }

    public bool HasSkill(SkillId skill)
    {
        int skillIndex = (int)skill;
        if (skillIndex < numberOfSkills)
        {
            return mySkills[skillIndex];
        }
        return false;
    }

    public int GetSkillPoints() 
    {
        return skillpoints;
    }
}
