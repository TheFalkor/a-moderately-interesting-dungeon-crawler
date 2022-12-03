using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NegativeFeedback : MonoBehaviour , IPointerUpHandler
{
    private Button me;
    private AudioCore audioCore;
    // Start is called before the first frame update
    void Start()
    {
        me = gameObject.GetComponent<Button>();
        audioCore = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioCore>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!me.interactable)
            audioCore.PlaySFX("NEGATIVE_FEEDBACK");
    }
}
