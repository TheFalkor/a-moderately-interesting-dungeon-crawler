using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void WeaponAttackMode()
    {
        //
    }


    public void RotatePortrait(RectTransform transform)
    {
        transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);
    }
}
