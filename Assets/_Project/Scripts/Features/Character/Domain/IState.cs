namespace Project.Features.Character.Domain
{
    public interface IState
    {
        public void Enter(); // Enter the state.
        public void Update(); // Update every frame.
        public void FixedUpdate(); // FixedUpdate every physics frame.
        public void Exit(); // Exit the state.
    }
}