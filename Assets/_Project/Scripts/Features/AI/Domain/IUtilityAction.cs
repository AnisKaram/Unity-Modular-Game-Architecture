using UnityEngine;

namespace Project.Features.AI.Domain
{
    /// <summary>
    /// Represents a single behavior (Eat, Sleep, Attack, etc...).
    /// </summary>
    public interface IUtilityAction
    {
        public string Name { get; } // For debugging purposes only.
        public float Evaluate(); // Calculates the utility score.
        public void Execute(); // Performs the logic
        public void TriggerAnimation(INpcAnimator npcAnimator);
    }
}
