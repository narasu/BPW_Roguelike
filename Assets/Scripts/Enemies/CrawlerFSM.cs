using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerFSM
{
    public Crawler owner { get; private set; }
    private Dictionary<EnemyStateType, CrawlerState> states;
    public EnemyStateType CurrentStateType { get; private set; }
    private CrawlerState currentState;
    private CrawlerState previousState;

    public void Initialize(Crawler _owner)
    {
        owner = _owner;
        states = new Dictionary<EnemyStateType, CrawlerState>();
    }

    public void AddState(EnemyStateType newType, CrawlerState newState)
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

    public CrawlerState GetState(EnemyStateType type)
    {
        if (!states.ContainsKey(type))
        {
            return null;
        }
        return states[type];
    }
}
