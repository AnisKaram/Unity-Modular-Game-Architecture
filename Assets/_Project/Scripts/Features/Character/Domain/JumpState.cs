using System;
using Project.Features.Character.Data;
using Project.Features.Character.View;
using UnityEngine;

namespace Project.Features.Character.Domain
{
    public class JumpState : IState
    {
        private readonly Rigidbody m_Rigidbody;
        private readonly PlayerSettingsSO m_PlayerSettings;
        private readonly PlayerInputReader m_PlayerInputReader;
        private readonly Transform m_ObjectTransform;

        public event Action OnLand;
        
        public JumpState(PlayerSettingsSO playerSettings, PlayerInputReader playerInputReader, Rigidbody rigidbody, Transform objectTransform)
        {
            m_PlayerSettings = playerSettings;
            m_PlayerInputReader = playerInputReader;
            m_Rigidbody = rigidbody;
            m_ObjectTransform = objectTransform;
        }
        
        public void Enter()
        {
            Debug.Log("Enter Jump State");
            
            // Reset the velocity
            Vector3 currentVelocity = m_Rigidbody.linearVelocity;
            m_Rigidbody.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
            
            // Add Force
            m_Rigidbody.AddForce(Vector3.up * m_PlayerSettings.jumpForce, ForceMode.Impulse);
        }

        public void Update()
        {
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
            if (m_Rigidbody.linearVelocity.y < 0)
            {
                if (Physics.Raycast(m_ObjectTransform.position, Vector3.down, m_PlayerSettings.jumpRaycastDistance, m_PlayerSettings.groundLayer))
                {
                    OnLand?.Invoke();
                    Debug.Log("Landed on ground");
                }
            }
        }

        public void Exit()
        {
            Debug.Log("Exit Jump State");
        }
    }
}
