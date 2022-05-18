using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRelative2Mouse : MonoBehaviour
{

    public Vector3 pz;
    public Vector3 startPos;

    public float moveModifier;

    void Start()
    {
        startPos = transform.position;
        
    }

    void Update()
    {
        Vector3 pz = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Debug.Log("Mouse Position: " + pz);

        pz.z = -10;

        transform.position = Vector3.Lerp(transform.position, pz, Time.deltaTime * moveModifier);
        //transform.position = new Vector3(startPos.x + (pz.x * moveModifier), startPos.y + (pz.y * (moveModifier/2)), 0);
        //move based on the starting position and its modified value.
    }

}