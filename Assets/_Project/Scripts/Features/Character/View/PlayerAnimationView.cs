using UnityEngine;
using VContainer;

namespace Project.Features.Character
{
    public class PlayerAnimationView : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        
        private readonly int m_RunningHash = Animator.StringToHash("IsRunning");
        private readonly int m_JumpingHash = Animator.StringToHash("Jumped");
        private readonly int m_LandingHash = Animator.StringToHash("Landed");
        
        private PlayerController m_PlayerController;
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            m_PlayerController = playerController;
        }

        private void Awake()
        {
            m_PlayerController.OnIdle += PlayerController_OnIdle;
            m_PlayerController.OnMove += PlayerController_OnMove;
            m_PlayerController.OnJump += PlayerController_OnJump;
            m_PlayerController.OnLand += PlayerController_OnLand;
        }
        private void OnDestroy()
        {
            m_PlayerController.OnIdle -= PlayerController_OnIdle;
            m_PlayerController.OnMove -= PlayerController_OnMove;
            m_PlayerController.OnJump -= PlayerController_OnJump;
            m_PlayerController.OnLand -= PlayerController_OnLand;
        }

        private void PlayerController_OnIdle()
        {
            m_Animator.SetBool(m_RunningHash, false);
        }
        
        private void PlayerController_OnMove()
        {
            m_Animator.SetBool(m_RunningHash, true);
        }
        
        private void PlayerController_OnJump()
        {
            m_Animator.SetTrigger(m_JumpingHash);
        }
        
        private void PlayerController_OnLand()
        {
            m_Animator.SetTrigger(m_LandingHash);
        }
    }
}