using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int health;

    protected Transform player;

    protected virtual void Awake()
    {
        player = Player.Instance.gameObject.transform;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        // spawn ammo/health
    }
    public virtual void TakeDamage(int _damage)
    {
        Debug.Log(gameObject + " took " + _damage + " damage");

        health -= _damage;

        if (health<=0)
        {
            Die();
        }
    }
}
