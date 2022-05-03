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

    [Header("Audio")]
    private AudioKor audioKor;

    //public int damageAmount= 10;//this is temporary. Should use the barrels damage stat.
    public int range=1;//in tiles
    public bool timeBomb = false;// temporary
    bool hasExploded = false;
    GridManager theGrid;
    //Occupant ocupantThatIsMe;
    ExplosionType typeOfExplosion;
    float timer = 0;//temporary used for testing
    // Start is called before the first frame update
    void Start()
    {
        theGrid = GridManager.instance;
        typeOfExplosion = ExplosionType.SQUARE;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {if (timeBomb) 
        {
            timer += Time.deltaTime;
            if(timer > 1) 
            {
                Damage selfHit=new Damage();
                selfHit.damage = 5;
                selfHit.origin = originType;
                TakeDamage(selfHit);
            }
        }
        
    }

    public override void Initialize()
    {
        base.Initialize();

        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }
    public void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            //Damage barrelDamage = new Damage(baseMeleeDamage,originType);

            audioKor.PlaySFX("EXPLOSION");
            //no status effects
            //List<Tile> allHitTiles = new List<Tile>();
            //Vector2 myPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            //Vector2 positionToCheck = new Vector2();

            if (typeOfExplosion == ExplosionType.SQUARE)
            {
                
                // cock
            }

        }
    }

    protected override void Death()
    {
        
        Explode();
        base.Death();
    }

}
