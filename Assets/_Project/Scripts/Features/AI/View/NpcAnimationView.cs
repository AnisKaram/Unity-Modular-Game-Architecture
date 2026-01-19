using Project.Features.AI.Domain;
using UnityEngine;

namespace Project.Features.AI.View
{
    public class NpcAnimationView : MonoBehaviour, INpcAnimator
    {
        [SerializeField] private Animator m_Animator;
        
        private readonly int m_RunningHash = Animator.StringToHash("IsRunning");
        private readonly int m_EatingHash = Animator.StringToHash("IsEating");
        
        public void SetIsRunning(bool isRunning)
        {
            m_Animator.SetBool(m_RunningHash, isRunning);
        }

        public void TriggerIsEating()
        {
            m_Animator.SetTrigger(m_EatingHash);
        }
    }
}