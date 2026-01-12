namespace Project.Core.Interfaces
{
    public interface IInteractable
    {
        public void Interact();
        public string InteractionPrompt { get; }
    }
}