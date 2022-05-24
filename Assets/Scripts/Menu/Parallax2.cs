using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2 : MonoBehaviour
{

    [SerializeField] private float parallaxHorizontal;
    [SerializeField] private float parallaxVertical;
    [SerializeField] private float timeMultiplier;


    void LateUpdate()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.x *= parallaxHorizontal;
        target.y *= parallaxVertical;
        target += Camera.main.transform.position;
        target.z = 0;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * timeMultiplier);
    }
}