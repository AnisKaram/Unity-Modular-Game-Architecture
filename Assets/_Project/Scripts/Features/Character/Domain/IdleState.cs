using Project.Features.Character.View;
using UnityEngine;
using System;

namespace Project.Features.Character.Domain
{
    public class IdleState : IState
    {
        private readonly PlayerInputReader m_PlayerInputReader;

        public event Action OnMove;
        
        public IdleState(PlayerInputReader playerInputReader)
        {
            m_PlayerInputReader = playerInputReader;
        }
        
        public void Enter()
        {
            Debug.Log("Enter Idle State");
        }

        public void Update()
        {
            float moveOffset = 0.01f;
            if (m_PlayerInputReader.GetPlayerInputData().movement.sqrMagnitude > moveOffset)
            {
                Debug.Log("Should switch to Move state");
                OnMove?.Invoke();
            }
        }

        public void FixedUpdate()
        {
            
        }

        public void Exit()
        {
            Debug.Log("Exit Idle State");
        }
    }
}