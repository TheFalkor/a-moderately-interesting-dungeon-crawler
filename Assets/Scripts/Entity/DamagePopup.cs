using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public void Setup(int damage, bool isPlayer)
    {
        int factor = Random.Range(0, 2) == 1 ? -1 : 1;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(150 * factor, 600));
        Destroy(gameObject, 0.75f);

        if (isPlayer)
            GetComponent<TextMesh>().color = new Color(0, 185, 0);

        GetComponent<TextMesh>().text = "" + damage;
    }

}
