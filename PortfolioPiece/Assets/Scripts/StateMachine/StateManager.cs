using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Game;

namespace StateMachine
{
    public class StateManager
    {
        internal Player Player;
        internal IState currentState;
        internal IState previousState;
        
        internal Dictionary<States, IState> States;

        public StateManager(Player player)
        {
            Player = player;

            States = new Dictionary<States, IState>();
            States.Add(StateMachine.States.Idle, new Player_Idle(this));
            States.Add(StateMachine.States.Moving, new Player_Moving(this));      

            currentState = States[StateMachine.States.Idle];
        }

        public void ChangeState(States newState)
        {
            if (States[newState] == currentState) return;

            if (currentState != null)
            {
                previousState = currentState;
                currentState.Exit();
            }

            currentState = States[newState];
            currentState.Enter();
            
        }

        public void Process()
        {
            if (currentState != null)
            {
                currentState.Process();
            }
        }
    }

    public enum States
    {
        Idle,
        Moving,
    }
}