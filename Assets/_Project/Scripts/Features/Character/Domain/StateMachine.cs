namespace Project.Features.Character.Domain
{
    public class StateMachine
    {
        private IState m_CurrentState;
        
        public StateMachine(IState startingState)
        {
            m_CurrentState = startingState;
            m_CurrentState.Enter();
        }

        public void Update()
        {
            m_CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            m_CurrentState?.FixedUpdate();
        }

        public void ChangeState(IState newState)
        {
            m_CurrentState?.Exit();
            m_CurrentState = newState;
            m_CurrentState.Enter();
        }
    }
}