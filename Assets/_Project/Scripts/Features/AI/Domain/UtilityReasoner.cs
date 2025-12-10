using System.Collections.Generic;

namespace Project.Features.AI.Domain
{
    /// <summary>
    /// Represents a list of actions and decide which one is the best right now for the AI
    /// to execute.
    /// </summary>
    public class UtilityReasoner
    {
        private List<IUtilityAction> m_Actions;

        public UtilityReasoner(List<IUtilityAction> actions)
        {
            m_Actions = actions;
        }

        public IUtilityAction SelectBestAction()
        {
            IUtilityAction bestAction = null;
            float bestScore = 0f;
            
            foreach (IUtilityAction action in m_Actions)
            {
                float evaluation = action.Evaluate();

                if (evaluation > bestScore)
                {
                    bestScore = evaluation;
                    bestAction = action;
                }
            }
            return bestAction;
        }
    }
}