using Project.Features.Character.Domain;
using UnityEngine;

namespace Project.Features.Character.View
{
    public class PlayerInputReader : MonoBehaviour
    {
        private PlayerInputData m_PlayerInputData;

        private void Update()
        {
            // Movement
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            m_PlayerInputData.movement = new Vector2(x, y);
            
            // Jump
            m_PlayerInputData.jump = Input.GetButtonDown("Jump");
            
            // Attack
            m_PlayerInputData.attack = Input.GetButtonDown("Fire1");
            
            // Interact
            m_PlayerInputData.interact = Input.GetButtonDown("Interact");
        }
        
        public PlayerInputData GetPlayerInputData() => m_PlayerInputData;
    }
}