using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public void Setup(int damage, DamageOrigin origin)
    {
        float factor = Random.Range(-1.0f, 1.0f);

        if (factor < 0)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-25 + 100 * factor, 600));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector2(25 + 100 * factor, 600));

        TextMesh mesh = transform.GetChild(0).GetComponent<TextMesh>();
        if (origin == DamageOrigin.ENEMY)
            mesh.color = new Color(180 / 255f, 120 / 255f, 230 / 255f);
        else if (origin == DamageOrigin.FRIENDLY)
            mesh.color = new Color(1, 75 / 255f, 75 / 255f);
       else
            mesh.color = new Color(1, 1, 1);


        mesh.text = "-" + damage;

        Destroy(gameObject, 0.75f);
    }

}
