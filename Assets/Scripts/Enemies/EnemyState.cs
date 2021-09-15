using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State<Enemy>
{
    public FSM<Enemy> owner;
    public EnemyState(FSM<Enemy> _owner)
    {
        owner = _owner;
    }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(FSM<Enemy> _owner) : base(_owner) { }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
