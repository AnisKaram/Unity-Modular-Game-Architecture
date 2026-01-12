using System;
using Project.Core.Interfaces;
using UnityEngine;
using VContainer;
using Vector3 = UnityEngine.Vector3;

namespace Project.Features.Character.View
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_InteractionRange = 2f;
        [SerializeField] private LayerMask m_InteractionLayerMask;
        
        private PlayerInputReader m_PlayerInputReader;
        
        private IInteractable m_CurrentInteractable;
        private IInteractable m_LastInteractable;
        
        public event Action<IInteractable> OnInteractableChanged;

        [Inject]
        public void Construct(PlayerInputReader playerInputReader)
        {
            m_PlayerInputReader = playerInputReader;
        }

        private void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(
                transform.position + (transform.forward * 1f),
                m_InteractionRange,
                m_InteractionLayerMask
            );

            IInteractable foundInteractable = null;
            if (hitColliders.Length > 0)
            {
                hitColliders[0].TryGetComponent(out foundInteractable);
            }

            if (foundInteractable != m_LastInteractable)
            {
                m_LastInteractable = foundInteractable;
                OnInteractableChanged?.Invoke(foundInteractable);
            }
            m_CurrentInteractable = foundInteractable;

            
            if (m_PlayerInputReader.GetPlayerInputData().interact && m_CurrentInteractable != null)
            {
                m_CurrentInteractable.Interact();
            }
        }
    }
}