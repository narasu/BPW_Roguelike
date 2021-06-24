using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Crawler : Enemy
{
    private CrawlerFSM fsm;
    public NavMeshAgent navMeshAgent;
    protected override void Awake()
    {
        base.Awake();
        fsm = new CrawlerFSM();
        fsm.Initialize(this);
        fsm.AddState(EnemyStateType.Idle, new IdleState());
        fsm.AddState(EnemyStateType.Chase, new ChaseState());
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        fsm.GotoState(EnemyStateType.Idle);
    }

    private void Update()
    {
        fsm.UpdateState();
    }
}

