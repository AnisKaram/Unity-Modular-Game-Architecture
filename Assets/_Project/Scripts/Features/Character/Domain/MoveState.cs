using Project.Features.Character.Data;
using Project.Core.Input;
using UnityEngine;
using System;

namespace Project.Features.Character.Domain
{
    public class MoveState : IState
    {
        private readonly PlayerSettingsSO m_PlayerSettings;
        private readonly PlayerInputReader m_PlayerInputReader;
        private readonly Transform m_ObjectTransform;

        public event Action OnIdle;
        public event Action OnJump;

        public MoveState(PlayerSettingsSO playerSettings, PlayerInputReader playerInputReader, Transform objectTransform)
        {
            m_PlayerSettings = playerSettings;
            m_PlayerInputReader = playerInputReader;
            m_ObjectTransform = objectTransform;
        }
        
        public void Enter()
        {
            Debug.Log("Enter MoveState");
        }

        public void Update()
        {
            if (m_PlayerInputReader.GetPlayerInputData().movement.sqrMagnitude == 0)
            {
                OnIdle?.Invoke();
                return;
            }

            if (m_PlayerInputReader.GetPlayerInputData().jump)
            {
                OnJump?.Invoke();
                return;
            }
            
            // Move
            Vector2 movementVector2 = m_PlayerInputReader.GetPlayerInputData().movement;
            Vector3 movementVector3 = new Vector3(movementVector2.x, 0, movementVector2.y);
            m_ObjectTransform.position += movementVector3 * (m_PlayerSettings.movementSpeed * Time.deltaTime);

            // Rotate when moving only
            if (movementVector3 != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movementVector3);
                m_ObjectTransform.rotation = Quaternion.Slerp(
                    m_ObjectTransform.rotation,
                    targetRotation,
                    m_PlayerSettings.rotationSpeed * Time.deltaTime);
            }
        }

        public void FixedUpdate()
        {

        }

        public void Exit()
        {
            Debug.Log("Exit MoveState");
        }
    }
}