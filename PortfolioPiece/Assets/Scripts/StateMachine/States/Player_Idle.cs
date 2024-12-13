using UnityEngine;
using StateMachine;
using Game;
public class Player_Idle : IState
{

    StateManager StateManager;
    Player player;

    public Player_Idle(StateManager stateManager)
    {
        StateManager = stateManager;
        player = stateManager.Player;
    }
    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Process()
    {
    }
    }
