using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    public T Owner { get; protected set; }
    public void Initialize(T _owner)
    {
        Owner = _owner;
    }
    public void AddState(State<T> _state)
    {
        allStates.Add(_state.GetType(), _state);

    }
    public void Update()
    {
        currentState?.OnUpdate();
    }
    public void SwitchState(System.Type _type)
    {
        currentState?.OnExit();
        currentState = allStates[_type];
        currentState?.OnEnter();
    }
    
    private State<T> currentState;
    private Dictionary<System.Type, State<T>> allStates = new Dictionary<System.Type, State<T>>();
    

}
