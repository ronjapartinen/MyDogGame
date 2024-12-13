namespace StateMachine
{
    public interface IState
    {
        public void Enter();
        public void Process();
        public void Exit();
    }
}
