using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int health;
    protected Transform player;
    protected EnemyFSM fsm;

    protected virtual void Awake()
    {
        player = Player.Instance.gameObject.transform;
        fsm = new EnemyFSM();
        fsm.Initialize(this);
        fsm.AddState(EnemyStateType.Idle, new IdleState());
    }

    private void Start()
    {
        fsm.GotoState(EnemyStateType.Idle);
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
