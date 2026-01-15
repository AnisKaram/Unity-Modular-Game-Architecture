using Project.Core.Input;
using Project.Features.Character.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Features.Character.View
{
    public class PlayerInputReader : MonoBehaviour
    {
        private PlayerInputData m_PlayerInputData;
        private GameControls m_GameControls;

        private void Awake()
        {
            m_GameControls = new GameControls();
            
            Debug.Log("Controls: GameControls Created!");
        }
        private void OnEnable()
        {
            m_GameControls.Player.Move.performed += OnMovePerformed;
            m_GameControls.Player.Move.canceled += OnMoveCanceled;

            m_GameControls.Player.Jump.started += OnJumpStarted;
            m_GameControls.Player.Jump.canceled += OnJumpCanceled;

            m_GameControls.Player.Attack.started += OnAttackStarted;
            m_GameControls.Player.Attack.canceled += OnAttackCanceled;

            m_GameControls.Player.Interact.started += OnInteractStarted;
            m_GameControls.Player.Interact.canceled += OnInteractCanceled;

            m_GameControls.Player.Rewind.started += OnRewindStarted;
            m_GameControls.Player.Rewind.canceled += OnRewindCanceled;

            m_GameControls.Player.Enable();
            
            Debug.Log("Controls: GameControls Enabled and Subscribed to Events!");
        }
        private void OnDisable()
        {
            m_GameControls.Player.Disable();

            m_GameControls.Player.Move.performed -= OnMovePerformed;
            m_GameControls.Player.Move.canceled -= OnMoveCanceled;

            m_GameControls.Player.Jump.started -= OnJumpStarted;
            m_GameControls.Player.Jump.canceled -= OnJumpCanceled;

            m_GameControls.Player.Attack.started -= OnAttackStarted;
            m_GameControls.Player.Attack.canceled -= OnAttackCanceled;

            m_GameControls.Player.Interact.started -= OnInteractStarted;
            m_GameControls.Player.Interact.canceled -= OnInteractCanceled;

            m_GameControls.Player.Rewind.started -= OnRewindStarted;
            m_GameControls.Player.Rewind.canceled -= OnRewindCanceled;
            
            Debug.Log("Controls: GameControls Disabled and Unsubscribed from Events!");
        }
        private void OnDestroy()
        {
            m_GameControls?.Dispose();
            m_GameControls = null;
            
            Debug.Log("Controls: GameControls Destroyed and set to Null!");
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx) =>
            m_PlayerInputData.movement = ctx.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext ctx) =>
            m_PlayerInputData.movement = ctx.ReadValue<Vector2>();

        private void OnJumpStarted(InputAction.CallbackContext ctx) => m_PlayerInputData.jump = true;
        private void OnJumpCanceled(InputAction.CallbackContext ctx) => m_PlayerInputData.jump = false;

        private void OnAttackStarted(InputAction.CallbackContext ctx) => m_PlayerInputData.attack = true;
        private void OnAttackCanceled(InputAction.CallbackContext ctx) => m_PlayerInputData.attack = false;

        private void OnInteractStarted(InputAction.CallbackContext ctx) => m_PlayerInputData.interact = true;
        private void OnInteractCanceled(InputAction.CallbackContext ctx) => m_PlayerInputData.interact = false;

        private void OnRewindStarted(InputAction.CallbackContext ctx) => m_PlayerInputData.isRewinding = true;
        private void OnRewindCanceled(InputAction.CallbackContext ctx) => m_PlayerInputData.isRewinding = false;

        public PlayerInputData GetPlayerInputData() => m_PlayerInputData;
    }
}