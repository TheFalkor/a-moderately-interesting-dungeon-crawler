using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : Occupant
{
    [Header("Audio")]
    private AudioKor audioKor;


    public override void Initialize()
    {
        base.Initialize();

        audioKor = GameObject.FindGameObjectWithTag("Manager").GetComponent<AudioKor>();
    }

    protected override void Death()
    {
        audioKor.PlaySFX("EXPLOSION");

        foreach (Tile t in currentTile.orthogonalNeighbors)
        {
            Attack(t, new Damage(baseMeleeDamage, originType));
        }

        foreach (Tile t in currentTile.diagonalNeighbors)
        {
            Attack(t, new Damage(baseMeleeDamage, originType));
        }

        base.Death();
    }
}
