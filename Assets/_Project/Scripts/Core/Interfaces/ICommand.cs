namespace Project.Core.Interfaces
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}