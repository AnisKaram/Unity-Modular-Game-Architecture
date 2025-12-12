namespace Project.Features.AI.Domain
{
    public class FixedConsideration : IConsideration
    {
        public float Score { get; }

        public FixedConsideration(float score)
        {
            Score = score;
        }
    }
}
