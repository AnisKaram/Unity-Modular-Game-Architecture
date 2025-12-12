using System.Collections.Generic;
using Project.Features.AI.Actions;
using Project.Features.AI.Domain;
using UnityEngine;

namespace Project.Features.AI.Components
{
    public class NpcController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NpcContext m_NpcContext;
        
        [Header("Curves")]
        [SerializeField] private AnimationCurve m_ChaseCurve;
        
        private UtilityReasoner m_Brain;
        private IUtilityAction m_CurrentAction;

        private void Start()
        {
            // Curve Consideration.
            CurveConsideration chaseConsideration = new CurveConsideration(
                "Chase Curve", m_ChaseCurve,
                () => Vector3.Distance(transform.position, m_NpcContext.Target.position),
                0, 20);
            
            // List of Chase Considerations.
            List<IConsideration> chaseConsiderations = new List<IConsideration>
            {
                chaseConsideration,
            };

            // MoveToTarget Action.
            MoveToTargetAction chaseAction = new MoveToTargetAction(m_NpcContext, chaseConsiderations);

            // All actions.
            List<IUtilityAction> actions = new List<IUtilityAction>()
            {
                chaseAction,
            };
            
            // Defining the reasoner (brain).
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