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
        [SerializeField] private AnimationCurve m_HungerCurve;
        [SerializeField] private AnimationCurve m_HasAppleCurve;
        
        private UtilityReasoner m_Brain;
        private IUtilityAction m_CurrentAction;

        private void Start()
        {
            // Chase curve consideration.
            CurveConsideration chaseConsideration = new CurveConsideration(
                "Chase Curve", m_ChaseCurve,
                () => Vector3.Distance(transform.position, m_NpcContext.Target.position),
                0, 20);
            
            // Fixed considerations
            FixedConsideration idleFixedConsideration = new FixedConsideration(0.2f);
            
            // Eat curve considerations.
            CurveConsideration hungerConsideration = new CurveConsideration(
                "Hunger Curve", m_HungerCurve,
                () => m_NpcContext.Stats.Hunger,
                0, 100);
            CurveConsideration appleConsideration = new CurveConsideration(
                "Apple Curve", m_HasAppleCurve,
                () => m_NpcContext.Inventory.GetTotalQuantity(m_NpcContext.GetApple().ID),
                0, 1);
            
            // List of chase, idle, and eat considerations.
            List<IConsideration> chaseConsiderations = new List<IConsideration> { chaseConsideration };
            List<IConsideration> idleConsiderations = new List<IConsideration>() { idleFixedConsideration };
            List<IConsideration> eatConsiderations = new List<IConsideration>() { hungerConsideration, appleConsideration };
            
            // MoveToTarget and Idle Actions.
            // NOTE: We give each action the Consideration THAT matters to the action only.
            MoveToTargetAction chaseAction = new MoveToTargetAction(m_NpcContext, chaseConsiderations);
            IdleAction idleAction = new IdleAction(m_NpcContext, idleConsiderations);
            EatAction eatAction = new EatAction(m_NpcContext, eatConsiderations, m_NpcContext.GetApple().ID);
            
            // All actions.
            List<IUtilityAction> actions = new List<IUtilityAction>()
            {
                idleAction,
                chaseAction,
                eatAction
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