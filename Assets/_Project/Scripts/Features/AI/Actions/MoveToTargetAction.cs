using System.Collections.Generic;
using Project.Features.AI.Components;
using Project.Features.AI.Domain;

namespace Project.Features.AI.Actions
{
    public class MoveToTargetAction : UtilityActionBase
    {
        private readonly NpcContext m_Context;

        public MoveToTargetAction(NpcContext context, List<IConsideration> considerations) 
            : base(considerations, "Move to Target")
        {
            m_Context = context;
        }

        public override void Execute()
        {
            m_Context.Agent.SetDestination(m_Context.Target.position);
        }
    }
}