using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    public float detectionDistance;
    
    public Enemy()
    {
        fsm = new FSM<Enemy>();
        fsm.Initialize(this);
    }

    public void SetSpawnPoint(SpawnPoint _spawnPoint) => spawnPoint = _spawnPoint;

    public virtual void TakeDamage(int _damage)
    {
        Debug.Log(gameObject + " took " + _damage + " damage");

        health -= _damage;

        if (health<=0)
        {
            Die();
        }
    }

    private FSM<Enemy> fsm;
    private SpawnPoint spawnPoint;

    [SerializeField] protected int health;

    protected virtual void Die()
    {
        //EventManager.RaiseEvent(EventType.ENEMY_KILLED);
        //spawnPoint.Empty = true;
        Destroy(gameObject);

        // spawn ammo/health
    }
}
