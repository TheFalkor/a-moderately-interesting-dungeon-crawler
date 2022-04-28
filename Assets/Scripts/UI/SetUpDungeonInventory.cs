using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpDungeonInventory : MonoBehaviour
{
    public GameObject buttonPrefab;
    protected Sprite defaultSprite;
    public Player playerScript;
    public int amountOfItemslots=48;
    protected List<Image> buttonImages=new List<Image>();
    protected List<Text> buttonTexts = new List<Text>();
    protected float buttonWidth;
    protected float buttonHeight;
    protected float containerWidth;
    protected float containerHeight;
    public  DungeonEquipmentBar equipmentBar;
    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() 
    {
        defaultSprite = buttonPrefab.GetComponentInChildren<Image>().sprite;
        playerScript.SetUpInventory();
        Rect rectangle = buttonPrefab.GetComponent<RectTransform>().rect;
        buttonWidth = rectangle.width;
        buttonHeight = rectangle.height;
        Rect containerRectangle = gameObject.GetComponent<RectTransform>().rect;
        containerHeight = containerRectangle.height;
        containerWidth = containerRectangle.width;
        for (int i = 0; i < amountOfItemslots; i++)
        {
            CreateButton(i);
        }
        UpdateSprites();
    }
   

    protected virtual void CreateButton(int index) 
    {
        
        int widthInButtons = (int)(containerWidth/buttonWidth);
        if (widthInButtons < 1)
        {
            widthInButtons = 1;
        }
        float xPos = -(containerWidth / 2 - buttonWidth / 2 - buttonWidth * (index%widthInButtons));
        float yPos = (containerHeight / 2 - buttonHeight/2  -buttonHeight*Mathf.Floor((index/widthInButtons)));
        CreateButtonAtPos(xPos, yPos, index);
    }
    protected void CreateButtonAtPos(float xPos,float yPos,int index) 
    {
        GameObject theButton = Instantiate(buttonPrefab, gameObject.transform);
        theButton.transform.localPosition = new Vector2(xPos, yPos);
        Button buttonCoponent = theButton.GetComponent<Button>();
        WireUpButton(buttonCoponent,index);
        
        for(int i = 0; i < theButton.transform.childCount;i++) 
        {
            GameObject childOfButton = theButton.transform.GetChild(i).gameObject;
            switch (childOfButton.name) 
            {
                case "Icon":buttonImages.Add(childOfButton.GetComponent<Image>());break;
                case "Stack Text": buttonTexts.Add(childOfButton.GetComponent<Text>());break;
                default:Debug.LogError("Check if "+childOfButton.name+ " is named correctly"); break;
            }
        }
    }
    protected virtual void WireUpButton(Button button,int index) 
    {
        button.onClick.AddListener(delegate { playerScript.UseItem(index); });
        button.onClick.AddListener(delegate { UpdateSprites(); });
    }

    public virtual void UpdateSprites() 
    {
        for(int i = 0; i < buttonImages.Count; i++) 
        {
            Sprite spr = playerScript.GetItemImage(i);
            if (spr == null) 
            {
                spr = defaultSprite;
            }
            buttonImages[i].sprite =spr ;

            int stackSize = playerScript.GetStackSize(i);
            string tempText = "";
            if (stackSize != 0&&stackSize!=1) 
            {
                tempText = stackSize.ToString();
            }
            buttonTexts[i].text = tempText;
        }

        if (equipmentBar != null) 
        {
            equipmentBar.UpdateSprites();
        }
    }
}
