using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType { Setup, Playing, Win, Lose }
public abstract class GameState
{
    protected GameFSM owner;
    protected GameManager gameManager;
    protected Player player;

    public void Initialize(GameFSM _owner)
    {
        this.owner = _owner;
        gameManager = _owner.owner;
        player = Player.Instance;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
public class SetupState : GameState
{
    public override void Enter()
    {

    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
public class PlayingState : GameState
{
    public override void Enter()
    {
        
    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {

    }
}
public class WinState : GameState
{
    public override void Enter()
    {

    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}
public class LoseState : GameState
{
    public override void Enter()
    {

    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}