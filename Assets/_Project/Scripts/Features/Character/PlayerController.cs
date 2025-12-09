using Project.Features.Character.Data;
using Project.Features.Character.Domain;
using Project.Features.Character.View;
using UnityEngine;
using VContainer;

namespace Project.Features.Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_Rigidbody;
        
        private PlayerInputReader m_PlayerInputReader;
        private PlayerSettingsSO m_PlayerSettings;
        
        private IdleState m_IdleState;
        private MoveState m_MoveState;
        private JumpState m_JumpState;
        
        private StateMachine m_StateMachineInstance;
        
        [Inject]
        public void Construct(PlayerInputReader playerInputReader, PlayerSettingsSO playerSettings)
        {
            m_PlayerInputReader = playerInputReader;
            m_PlayerSettings = playerSettings;
        }

        private void Start()
        {
            m_IdleState = new IdleState(m_PlayerInputReader);
            m_MoveState = new MoveState(m_PlayerSettings, m_PlayerInputReader, transform);
            m_JumpState = new JumpState(m_PlayerSettings, m_Rigidbody, transform);

            m_IdleState.OnMove += IdleState_OnMove;
            m_IdleState.OnJump += IdleState_OnJump;
            
            m_MoveState.OnIdle += MoveState_OnIdle;
            m_MoveState.OnJump += MoveState_OnJump;
            
            m_JumpState.OnLand += JumpState_OnLand;

            m_StateMachineInstance = new StateMachine(m_IdleState);
        }
        private void Update()
        {
            m_StateMachineInstance.Update();
        }
        private void FixedUpdate()
        {
            m_StateMachineInstance.FixedUpdate();
        }
        private void OnDestroy()
        {
            m_IdleState.OnMove -= IdleState_OnMove;
            m_IdleState.OnJump -= IdleState_OnJump;
            
            m_MoveState.OnIdle -= MoveState_OnIdle;
            m_MoveState.OnJump -= MoveState_OnJump;
            
            m_JumpState.OnLand -= JumpState_OnLand;
        }

        private void IdleState_OnMove()
        {
            m_StateMachineInstance.ChangeState(m_MoveState);
        }

        private void IdleState_OnJump()
        {
            m_StateMachineInstance.ChangeState(m_JumpState);
        }

        private void MoveState_OnIdle()
        {
            m_StateMachineInstance.ChangeState(m_IdleState);
        }

        private void MoveState_OnJump()
        {
            m_StateMachineInstance.ChangeState(m_JumpState);
        }

        private void JumpState_OnLand()
        {
            m_StateMachineInstance.ChangeState(m_IdleState);
        }
    }
}