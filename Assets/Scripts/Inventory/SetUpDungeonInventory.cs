using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpDungeonInventory : MonoBehaviour
{
    public GameObject buttonPrefab;
    Sprite defaultSprite;
    public Player playerScript;
    public int amountOfItemslots=48;
    List<Image> buttonImages=new List<Image>();
    // Start is called before the first frame update
    void Start()
    {
        defaultSprite = buttonPrefab.GetComponent<Image>().sprite;
        playerScript.SetUpInventory();
        for(int i = 0; i < amountOfItemslots; i++) 

        {
            CreateButton(i);
        }
        UpdateSprites();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateButton(int index) 
    {
        Rect rectangle = gameObject.GetComponent<RectTransform>().rect;
        GameObject theButton = Instantiate(buttonPrefab, gameObject.transform);
        Rect buttonRect = theButton.GetComponent<RectTransform>().rect;
        int widthInButtons = (int)(rectangle.width/buttonRect.width);
        float xPos = -(rectangle.width / 2 - buttonRect.width / 2 -buttonRect.width*(index%widthInButtons));
        float yPos = (rectangle.height / 2 - buttonRect.height / 2-buttonRect.height*Mathf.Floor((index/widthInButtons)));
        theButton.transform.localPosition =new Vector2(xPos,yPos);
        Button buttonCoponent = theButton.GetComponent<Button>();
        buttonCoponent.onClick.AddListener(delegate { playerScript.UseItem(index); });
        buttonCoponent.onClick.AddListener(delegate { UpdateSprites(); });
        
        buttonImages.Add(theButton.GetComponent<Image>());

    }
    public void UpdateSprites() 
    {
        for(int i = 0; i < buttonImages.Count; i++) 
        {
            Sprite spr = playerScript.GetItemImage(i);
            if (spr == null) 
            {
                spr = defaultSprite;
            }
            buttonImages[i].sprite =spr ;
        }
    }
}
