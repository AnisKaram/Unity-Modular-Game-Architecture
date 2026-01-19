using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.AI.Domain
{
    public abstract class UtilityActionBase : IUtilityAction
    {
        private readonly List<IConsideration> m_Considerations;
        public string Name { get; }

        protected UtilityActionBase(List<IConsideration> considerations, string name)
        {
            m_Considerations = considerations;
            Name = name;
        }
        
        public float Evaluate()
        {
            float finalScore = 1f;

            foreach (IConsideration consideration in m_Considerations)
            {
                finalScore *= consideration.Score;

                // If one consideration is 0 then the whole action is 0 (impossible).
                if (finalScore == 0)
                {
                    break;
                }
            }
            
            // Clamp the finalScore between 0 and 1.
            return Mathf.Clamp01(finalScore);
        }

        public abstract void Execute();
        public abstract void TriggerAnimation(Animator animator);
    }
}
