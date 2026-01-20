using System;
using Project.Core.Interfaces;
using UnityEngine;
using VContainer;
using Project.Core.Input;

namespace Project.Features.Character.View
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_InteractionRange = 2f;
        [SerializeField] private LayerMask m_InteractionLayerMask;
        
        private PlayerInputReader m_PlayerInputReader;
        
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
                float nearestDistance = Mathf.Infinity;
                IInteractable nearestInteractable = null;

                // Find the nearest interactable.
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].TryGetComponent(out IInteractable currentInteractable))
                    {
                        // Avoiding Vector3.Distance because it uses Mathf.Sqrt which is slow if we are constantly checking
                        float distSqr = (hitColliders[i].transform.position - transform.position).sqrMagnitude;

                        if (distSqr < nearestDistance)
                        {
                            nearestDistance = distSqr;
                            nearestInteractable = currentInteractable;
                        }
                    }
                }

                foundInteractable = nearestInteractable;
            }

            if (foundInteractable != m_LastInteractable)
            {
                m_LastInteractable = foundInteractable;
                OnInteractableChanged?.Invoke(foundInteractable);
            }
            
            if (m_PlayerInputReader.GetPlayerInputData().interact && foundInteractable != null)
            {
                foundInteractable.Interact();
            }
        }
    }
}