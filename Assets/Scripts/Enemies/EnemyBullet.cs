using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
