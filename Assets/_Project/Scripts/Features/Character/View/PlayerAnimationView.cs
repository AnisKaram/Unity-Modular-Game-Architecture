using UnityEngine;
using VContainer;

namespace Project.Features.Character
{
    public class PlayerAnimationView : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        
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
            m_Animator.SetBool("IsRunning", false);
        }
        
        private void PlayerController_OnMove()
        {
            m_Animator.SetBool("IsRunning", true);
        }
        
        private void PlayerController_OnJump()
        {
            m_Animator.SetTrigger("Jumped");
        }
        
        private void PlayerController_OnLand()
        {
            m_Animator.SetTrigger("Landed");
        }
    }
}