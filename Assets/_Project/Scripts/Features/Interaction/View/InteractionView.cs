using Project.Core.Interfaces;
using Project.Features.Character.View;
using TMPro;
using UnityEngine;
using VContainer;

namespace Project.Features.Interaction.View
{
    public class InteractionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_FeedbackText;
        
        private PlayerInteractor m_PlayerInteractor;

        [Inject]
        public void Construct(PlayerInteractor interactor)
        {
            m_PlayerInteractor = interactor;
        }

        private void Awake()
        {
            UpdateUI(null);
        }
        private void Start()
        {
            m_PlayerInteractor.OnInteractableChanged += PlayerInteractor_OnInteractableChanged;
        }
        private void OnDestroy()
        {
            m_PlayerInteractor.OnInteractableChanged -= PlayerInteractor_OnInteractableChanged;
        }

        private void PlayerInteractor_OnInteractableChanged(IInteractable interactedObject)
        {
            UpdateUI(interactedObject);
        }

        private void UpdateUI(IInteractable interactedObject)
        {
            if (interactedObject == null)
            {
                m_FeedbackText.text = "";
                m_FeedbackText.gameObject.SetActive(false);
                return;
            }
            
            m_FeedbackText.text = interactedObject.InteractionPrompt;
            m_FeedbackText.gameObject.SetActive(true);
        }
    }
}