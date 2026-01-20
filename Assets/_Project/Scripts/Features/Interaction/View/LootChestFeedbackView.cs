using Project.Features.Interaction.Components;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Project.Features.Interaction.View
{
    public class LootChestFeedbackView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LootChest m_LootChest;
        [SerializeField] private TextMeshPro m_FeedbackText;

        [Header("Animation Settings")] 
        [SerializeField] private float m_YOffset = 1f;
        [SerializeField] private float m_Duration = 0.5f;

        private void OnEnable()
        {
            m_LootChest.OnOpen += LootChest_OnOpen;
            Hide();
        }
        private void OnDisable()
        {
            m_LootChest.OnOpen -= LootChest_OnOpen;
            Hide();
            KillAnimation();
        }
        
        private void LootChest_OnOpen(object sender, LootChest.LootChestEventArgs eventArgs)
        {
            SetText(eventArgs.Name, eventArgs.Quantity);
            Show();
            StartAnimation();
        }

        private void SetText(string feedback, int quantity) =>
            m_FeedbackText.text = "+" + quantity + " " + feedback;
        
        private void Show() => m_FeedbackText.gameObject.SetActive(true);
        private void Hide() => m_FeedbackText.gameObject.SetActive(false);

        private void StartAnimation() =>
            m_FeedbackText.rectTransform.DOAnchorPosY(m_FeedbackText.rectTransform.anchoredPosition.y + m_YOffset, m_Duration);
        private void KillAnimation() => m_FeedbackText.rectTransform.DOKill();
    }
}