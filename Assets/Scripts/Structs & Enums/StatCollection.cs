using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCollection 
{
    const int numberOfStats = (int)StatType.count;
    int[] statModifiers = new int[numberOfStats];
    
    public StatCollection() 
    {
        AllStatsToZero();
    }
    public void AllStatsToZero() 
    {
        for (int i = 0; i < numberOfStats; i++)
        {
            statModifiers[i] = 0;
        }
    }

    public void CombineStats(StatCollection other) 
    {
        for(int i = 0; i < numberOfStats; i++) 
        {
            statModifiers[i] += other.GetStat(i);
        }
    }

    public void CopyValues(StatCollection from) 
    {
        for (int i = 0; i < numberOfStats; i++)
        {
            statModifiers[i] = from.GetStat(i);
        }
    }
    public StatCollection Clone() 
    {
        StatCollection copy = new StatCollection();
        copy.CopyValues(this);
        return copy;
    }
    public int GetStat(StatType stat) 
    {
        return GetStat((int)stat);
    }
    int GetStat(int index)
    {
        if (index < numberOfStats && index >= 0)
        {
            return statModifiers[index];
        }
        return 0;
    }

    public void SetStat(StatType stat,int value) 
    {
        SetStat((int)stat, value);
    }

    void SetStat(int index, int value)
    {
        if (index < numberOfStats && index >= 0) 
        {
            statModifiers[index] = value;
        }
    }
}
