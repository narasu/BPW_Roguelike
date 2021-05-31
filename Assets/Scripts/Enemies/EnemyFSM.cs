using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public Enemy owner { get; private set; }
    private Dictionary<EnemyStateType, EnemyState> states;
    public EnemyStateType CurrentStateType { get; private set; }
    private EnemyState currentState;
    private EnemyState previousState;

    public void Initialize(Enemy _owner)
    {
        owner = _owner;
        states = new Dictionary<EnemyStateType, EnemyState>();
    }

    public void AddState(EnemyStateType newType, EnemyState newState)
    {
        states.Add(newType, newState);
        states[newType].Initialize(this);
    }

    public void UpdateState()
    {
        currentState?.Update();
    }

    public void GotoState(EnemyStateType key)
    {
        if (!states.ContainsKey(key))
        {
            return;
        }

        currentState?.Exit();

        previousState = currentState;
        CurrentStateType = key;
        currentState = states[CurrentStateType];

        currentState.Enter();
    }

    public EnemyState GetState(EnemyStateType type)
    {
        if (!states.ContainsKey(type))
        {
            return null;
        }
        return states[type];
    }
}
