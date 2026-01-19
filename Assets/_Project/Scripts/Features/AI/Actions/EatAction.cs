using System.Collections.Generic;
using Project.Features.AI.Components;
using Project.Features.AI.Domain;
using UnityEngine;

namespace Project.Features.AI.Actions
{
    public class EatAction : UtilityActionBase
    {
        private readonly NpcContext m_Context;
        private readonly string m_FoodID;
        
        public EatAction(NpcContext context, List<IConsideration> considerations, string foodId) 
            : base(considerations, "Eat Action")
        {
            m_Context = context;
            m_FoodID = foodId;
        }

        public override void Execute()
        {
            // Check if available in the inventory.
            if (m_Context.Inventory.GetTotalQuantity(m_FoodID) > 0)
            {
                // Trying to consume it.
                if (m_Context.Inventory.ConsumeItem(m_FoodID, 1))
                {
                    // If consumed, we call Eat in the NpcStats.
                    m_Context.Stats.Eat(30f);
                    Debug.Log($"Ate an apple, Burp!");
                }
            }
        }

        public override void TriggerAnimation(INpcAnimator npcAnimator)
        {
            npcAnimator.TriggerIsEating();
        }
    }
}