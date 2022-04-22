using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public void Setup(int damage, bool isPlayer)
    {
        float factor = Random.Range(-1.0f, 1.0f);

        if (factor < 0)
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-25 + 100 * factor, 600));
        else
            GetComponent<Rigidbody2D>().AddForce(new Vector2(25 + 100 * factor, 600));

        TextMesh mesh = transform.GetChild(0).GetComponent<TextMesh>();
        if (isPlayer)
            mesh.color = new Color(0, 185 / 255f, 0);

        mesh.text = "-" + damage;

        Destroy(gameObject, 0.75f);
    }

}
