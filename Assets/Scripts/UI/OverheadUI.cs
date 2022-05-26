using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadUI : MonoBehaviour
{
    [Header("Sprite Reference")]
    [SerializeField] private Sprite deathmarkSprite;
    [SerializeField] private Sprite flutterSprite;
    [SerializeField] private Sprite slowerSprite;
    [SerializeField] private Sprite rootedSprite;

    private GameObject target;
    private GameObject filler;
    private GameObject fillerPointer;
    private List<SpriteRenderer> statusRenderers = new List<SpriteRenderer>();

    private const float MAX_SCALE = 5.5f;

    
    public void Initialize(GameObject target)
    {
        this.target = target;

        GetReferences();
    }

    private void Update()
    {
        if (target)
            transform.position = target.transform.position + new Vector3(0, 1.6f);
        else
            Destroy(gameObject);
    }

    public void UpdateHealthbar(float perc)
    {
        float barScale = MAX_SCALE * perc - 1;

        if (barScale < 0)
            barScale = 0;

        filler.transform.localScale = new Vector3(barScale, 1, 1);
        filler.transform.localPosition = new Vector3(-0.44f + 0.36f * (barScale / (MAX_SCALE - 1)), 0, 0);

        fillerPointer.transform.localPosition = new Vector3(-0.52f + 0.88f * perc, 0, 0);
    }

    public void UpdateStatusEffects(List<StatusEffect> list)
    {
        for (int i = 0; i < statusRenderers.Count; i++)
        {
            if (i >= statusRenderers.Count)
                break;

            if (i < list.Count)
            {
                Sprite sprite = null;

                switch (list[i].type)
                {
                    case StatusType.DEATHMARK:
                        sprite = deathmarkSprite;
                        break;
                    case StatusType.FLUTTER:
                        sprite = flutterSprite;
                        break;
                    case StatusType.SLOWED:
                        sprite = slowerSprite;
                        break;
                    case StatusType.ROOTED:
                        sprite = rootedSprite;
                        break;
                }

                statusRenderers[i].sprite = sprite;
                statusRenderers[i].gameObject.SetActive(true);
            }
            else
            {
                statusRenderers[i].gameObject.SetActive(false);
            }
        }
    }

    private void GetReferences()
    {
        filler = transform.GetChild(1).GetChild(0).gameObject;
        fillerPointer = transform.GetChild(1).GetChild(1).gameObject;

        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            statusRenderers.Add(transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>());
        }

    }
}
