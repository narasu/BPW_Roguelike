using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int health;
    public float detectionDistance;
    private SpawnPoint spawnPoint;

    //protected Transform player;

    public void Initialize(SpawnPoint _spawnPoint) => spawnPoint = _spawnPoint;

    private void Start()
    {
        
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        spawnPoint.Empty = true;
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
