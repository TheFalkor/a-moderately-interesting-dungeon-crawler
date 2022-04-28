using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRelative2Mouse : MonoBehaviour
{

    public Vector3 pz;
    public Vector3 startPos;

    public int moveModifier;

    void Start()
    {
        startPos = transform.position;
        
    }

    void Update()
    {
        Vector3 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        pz.z = 0;
        gameObject.transform.position = pz;
        //Debug.Log("Mouse Position: " + pz);

        transform.position = new Vector3(startPos.x + (pz.x * moveModifier), startPos.y + (pz.y * moveModifier), 0);
        //move based on the starting position and its modified value.
    }

}