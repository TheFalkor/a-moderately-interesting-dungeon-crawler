using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonFinalInventoryInterface : MonoBehaviour
{
    private Player player;
    public List<Image> inventoryImages = new List<Image>();
    public List<Text> inventoryText = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();

    }

    public void UpdateSprites() 
    {
        if (player == null) 
        {
            player = gameObject.GetComponent<Player>();
        }
        if (player != null) 
        {
            int i = 0;
            foreach (Image image in inventoryImages)
            {
                image.sprite = null;
                image.gameObject.SetActive(image.sprite);
                i++;
            }
            i = 0;
            foreach(Text text in inventoryText)
            {
                string tempString = "";
                int size=999;
                if (size != 1 && size != 0) 
                {
                    tempString = size.ToString();
                }
                text.text = tempString;
                i++;
            }

           
        }
        
    }
    
}
