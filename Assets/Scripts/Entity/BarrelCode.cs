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
    //public int damageAmount= 10;//this is temporary. Should use the barrels damage stat.
    public int range=1;//in tiles
    public bool timeBomb = false;// temporary
    bool hasExploded = false;
    GridManager theGrid;
    //Occupant ocupantThatIsMe;
    ExplosionType typeOfExplosion;
    float widthOfTile;
    float heightOfTile;
    float timer = 0;//temporary used for testing
    // Start is called before the first frame update
    void Start()
    {
        widthOfTile = 1;
        heightOfTile = 1;// both of these should read the size of tiles somewhere instead of being hard coded.
        theGrid = GridManager.instance;
        typeOfExplosion = ExplosionType.SQUARE;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timeBomb&&timer > 1) 
        {
            Damage selfHit=new Damage();
            selfHit.damage = 5;
            selfHit.origin = originType;
            TakeDamage(selfHit);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        currentTile.SetOccupant(null);
        currentTile = theGrid.GetTileWorldFuzzy(transform.position);
        transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y);
        currentTile.SetOccupant(this);
    }
    public void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            Damage barrelDamage = new Damage();
            barrelDamage.damage = baseMeleeDamage;
            barrelDamage.origin = originType;
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
                        positionToCheck.x = myPosition.x + i * widthOfTile;
                        positionToCheck.y = myPosition.y + j * heightOfTile;
                        Tile tileAtPositionToCheck = theGrid.GetTileWorldFuzzy(positionToCheck);//getTileWorldIsBugged
                        if (tileAtPositionToCheck != null)
                        {
                            allHitTiles.Add(tileAtPositionToCheck);
                        }
                    }
                }
            }
            foreach (Tile t in allHitTiles)
            {
                t.AttackTile(barrelDamage);
            }
        }
    }

    protected override void Death()
    {
        
        Explode();
        base.Death();
    }

}
