using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NegativeFeedback : MonoBehaviour , IPointerUpHandler
{
    private Button me;
    private AudioKor sfx;
    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<Button>();
        sfx = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!me.interactable)
            sfx.PlaySFX("NEGATIVE_FEEDBACK");
    }
}
