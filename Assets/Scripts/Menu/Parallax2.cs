using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2 : MonoBehaviour
{

    [SerializeField] private float parallaxEffectMultiplier;
    //private SpriteRenderer spriteRenderer;
    //[SerializeField] private Sprite[] alternativeSprites;
    private int index;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    //private bool instantiated = false;



    void Start()
    {
        cameraTransform = Camera.main.transform;
        //lastCameraPosition = cameraTransform.position;

        lastCameraPosition = transform.position;


        //Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        //Texture2D texture = sprite.texture;
        //textureUnitSizeX = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;


        //if (alternativeSprites.Length > 0)
        //{
        //    index = Random.Range(0, (alternativeSprites.Length));
        //    GetComponent<SpriteRenderer>().sprite = alternativeSprites[index];
        //}


    }

    /*  void FixedUpdate()
      {

          if (transform.position.x <= Input.mousePosition.x)
          {
              if (!instantiated)
              {
                  //Instantiate(gameObject, new Vector3((transform.position.x + textureUnitSizeX), transform.position.y, transform.position.z), Quaternion.identity);

                  instantiated = true;
              }
          }


      }
    */ //Time.deltaTime *

    void LateUpdate()
    {
        Vector3 cameraMovement = (Input.mousePosition / 5000)- lastCameraPosition;

        //transform.position = Vector3.Lerp(transform.position, cameraMovement, parallaxEffectMultiplier);

        transform.position = Vector3.Lerp(lastCameraPosition, cameraMovement, parallaxEffectMultiplier * Time.deltaTime);

        lastCameraPosition = transform.position;

        //if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        //{

          //  Destroy(this.gameObject);

        //S}

    }
}