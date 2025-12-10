using System.Collections.Generic;
using Project.Features.AI.Domain;
using UnityEngine;

namespace Project.Features.AI.Components
{
    public class NpcController : MonoBehaviour
    {
        private UtilityReasoner m_Brain;
        private IUtilityAction m_CurrentAction;

        private void Start()
        {
            List<IUtilityAction> actions = new List<IUtilityAction>();

            m_Brain = new UtilityReasoner(actions);
        }
        private void Update()
        {
            IUtilityAction bestAction = m_Brain.SelectBestAction();
            
            // Set it if not null and not equal.
            if (bestAction != null && m_CurrentAction != bestAction)
            {
                m_CurrentAction = bestAction;
                Debug.Log("NPC switched to " + m_CurrentAction.Name);
            }

            // Execute if not null.
            m_CurrentAction?.Execute();
        }
    }
}