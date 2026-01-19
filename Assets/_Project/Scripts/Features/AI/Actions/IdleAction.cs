using System.Collections.Generic;
using Project.Features.AI.Components;
using Project.Features.AI.Domain;
using UnityEngine;

namespace Project.Features.AI.Actions
{
    public class IdleAction : UtilityActionBase
    {
        private readonly NpcContext m_Context;
        
        public IdleAction(NpcContext context, List<IConsideration> considerations) 
            : base(considerations, "Idle Action")
        {
            m_Context = context;
        }

        public override void Execute()
        {
            m_Context.Agent.ResetPath();
        }

        public override void TriggerAnimation(Animator animator)
        {
            animator.SetBool("IsRunning", false);
        }
    }
}