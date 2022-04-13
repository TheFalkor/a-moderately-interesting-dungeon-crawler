using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrelCode : Occupant
{ private enum ExplosionType //this enum is temporary
    {
        SQUARE,
        DIAMOND,
        CIRCLE
    }
    public int damageAmount= 10;//this is temporary. Should use the barrels damage stat.
    public int range=1;//in tiles
    GridManager theGrid;
    //Occupant ocupantThatIsMe;
    ExplosionType typeOfExplosion;
    float widthOfTile;
    float heightOfTile;
    // Start is called before the first frame update
    void Start()
    {
        widthOfTile = 1;
        heightOfTile = 1;// both of these should read the size of tiles somewhere instead of being hard coded.

        //ocupantThatIsMe = gameObject.GetComponent<Occupant>();
        GameObject managerObject = GameObject.Find("Manager");
        theGrid = managerObject.GetComponent<GridManager>();
        typeOfExplosion = ExplosionType.SQUARE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool CheckIfShouldExpload()//not currently in use
    {
        /*if (ocupantThatIsMe != null) 
        {
           //check if health is less then zero. if true: Explode 
           // never mind. Explode will be called from the ocupant die function 
        
        }
        else 
        {
            Debug.LogError("No occupant script");
        }    */
        return false;
    }
    public void Explode()
    {
        Damage barrelDamage=new Damage();
        barrelDamage.damage = damageAmount;
        barrelDamage.origin = DamageOrigin.NEUTRAL;
        //no status effects
        List<Tile> allHitTiles = new List<Tile>();
        Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 positionToCheck = new Vector2();

        if (typeOfExplosion == ExplosionType.SQUARE)
        {
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    positionToCheck.x = myPosition.x + i*widthOfTile;
                    positionToCheck.y = myPosition.y + j*heightOfTile;
                    Tile tileAtPositionToCheck = theGrid.GetTileWorld(positionToCheck);//getTileWorldIsBugged
                    allHitTiles.Add(tileAtPositionToCheck);
                }
            }
        }
        foreach (Tile t in allHitTiles) 
        {
            t.AttackTile(barrelDamage);
        }
    }

    protected override void Death()
    {
        Explode();
    }

}
