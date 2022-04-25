using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonFinalInventoryInterface : MonoBehaviour
{
    private Player player;
    public List<Image> inventoryImages = new List<Image>();
    
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        
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
            foreach(Image image in inventoryImages) 
            {
                image.sprite = player.GetItemImage(i);
                image.gameObject.SetActive(image.sprite); 
                i++;
            }
        }
        
    }
    
}
