using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimManager : MonoBehaviour
{

    [SerializeField] float Speed;

    private Animator anim;

    [SerializeField] private float DelayBetweenAnimations;

    private float SecondsPassed = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Speed;
    }

    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            SecondsPassed += Time.deltaTime;
            if(SecondsPassed >= DelayBetweenAnimations)
            {
                anim.SetTrigger("BlinkingStarStart");
            }
        }
        else
        {
            anim.ResetTrigger("BlinkingStarStart");
            SecondsPassed = 0;
        }
    }
}
