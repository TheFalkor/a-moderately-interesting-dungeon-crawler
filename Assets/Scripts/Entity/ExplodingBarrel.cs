using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : Occupant
{
    [Header("VFX")]
    public GameObject explodeVFX;

    protected override void Death()
    {
        sfx.PlaySFX("EXPLOSION");

        foreach (Tile t in currentTile.orthogonalNeighbors)
        {
            Attack(t, new Damage(baseMeleeDamage, originType));

            GameObject vfx = Instantiate(explodeVFX, t.transform.position, Quaternion.identity);
            Destroy(vfx, 8 / 15f);
        }

        foreach (Tile t in currentTile.diagonalNeighbors)
        {
            Attack(t, new Damage(baseMeleeDamage, originType));

            GameObject vfx = Instantiate(explodeVFX, t.transform.position, Quaternion.identity);
            Destroy(vfx, 8 / 15f);
        }

        GameObject vfxself = Instantiate(explodeVFX, currentTile.transform.position, Quaternion.identity);
        Destroy(vfxself, 8 / 15f);

        base.Death();
    }
}
