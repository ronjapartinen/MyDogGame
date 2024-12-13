using StateMachine;
using UnityEngine;
using Game;
public class Player_Moving : IState
{
    StateManager StateManager;
    Player player;

    public Player_Moving(StateManager stateManager)
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

    public void Update()
    {
  
    }
}

