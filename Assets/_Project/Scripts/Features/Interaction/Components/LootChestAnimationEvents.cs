using System;
using UnityEngine;

namespace Project.Features.Interaction.Components
{
    public class LootChestAnimationEvents : MonoBehaviour
    {
        public event Action OnAnimationEnded;
        
        public void AnimationEnded()
        {
            OnAnimationEnded?.Invoke();
        }
    }
}