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

        [Inject]
        public void Construct(PlayerInputReader playerInputReader)
        {
            m_PlayerInputReader = playerInputReader;
        }

        private void Update()
        {
            // Raycast and store IInteractable item if possible.
            Collider[] hitColliders =  Physics.OverlapSphere(transform.position + (transform.forward * 1f), m_InteractionRange, m_InteractionLayerMask);
            if (hitColliders.Length > 0)
            {
                // Todo: For polish/later, we will loop through all the items and we find the closest one.
                
                if (hitColliders[0].TryGetComponent(out IInteractable item))
                {
                    m_CurrentInteractable = item;
                    Debug.Log($"Can Interact: {item.InteractionPrompt}");
                }
                else
                {
                    m_CurrentInteractable = null;
                }
            }
            
            // Interact.
            if (m_PlayerInputReader.GetPlayerInputData().interact && m_CurrentInteractable != null)
            {
                m_CurrentInteractable.Interact();
            }
        }
    }
}