using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 target;
    private Vector3 direction;

    public void setTarget(Vector3 t)
    {
        target = t + new Vector3(0, 0.5f, 0);
        direction = target - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

        StartCoroutine(Fly());
    }

    IEnumerator Fly()
    {
        float distanceMagnitude = 2;

        while (distanceMagnitude > 0.1f)
        {
            transform.position += direction * Time.deltaTime * speed;

            Vector3 distance = target - transform.position;

            distanceMagnitude = (target - transform.position).magnitude;
            yield return null;
        }

        Destroy(gameObject);
    }
}
