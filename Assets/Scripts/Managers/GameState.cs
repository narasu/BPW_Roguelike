using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType { Setup, Playing, Win, Lose }
public abstract class GameState : State<GameManager>
{
    public FSM<GameManager> owner;
    protected GameManager gameManager;
    protected Player player;

    public GameState(FSM<GameManager> _owner)
    {
        owner = _owner;
    }

    //public void Initialize(GameFSM _owner)
    //{
    //    this.owner = _owner;
    //    gameManager = _owner.owner;
    //    player = Player.Instance;
    //}

    public override void OnEnter() { }
    public override void OnUpdate() { }
    public override void OnExit() { }
}
public class SetupState : GameState
{
    public SetupState(FSM<GameManager> _owner) : base(_owner) { }
}
public class PlayingState : GameState
{
    public PlayingState(FSM<GameManager> _owner) : base(_owner) { }
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
public class WinState : GameState
{
    public WinState(FSM<GameManager> _owner) : base(_owner) { }
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
public class LoseState : GameState
{
    public LoseState(FSM<GameManager> _owner) : base(_owner) { }
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