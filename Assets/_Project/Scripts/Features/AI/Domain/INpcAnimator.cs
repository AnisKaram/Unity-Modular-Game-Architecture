namespace Project.Features.AI.Domain
{
    public interface INpcAnimator
    {
        public void SetIsRunning(bool isRunning);
        public void TriggerIsEating();
    }
}