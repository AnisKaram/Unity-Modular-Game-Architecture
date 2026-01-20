using Project.Features.Interaction.Components;
using TMPro;
using UnityEngine;

namespace Project.Features.Interaction.View
{
    public class LootChestFeedbackView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LootChest m_LootChest;
        [SerializeField] private TextMeshPro m_FeedbackText;

        private void OnEnable()
        {
            m_LootChest.OnOpen += LootChest_OnOpen;
            HideFeedback();
        }
        private void OnDisable()
        {
            m_LootChest.OnOpen -= LootChest_OnOpen;
            HideFeedback();
        }
        
        private void LootChest_OnOpen(object sender, LootChest.LootChestEventArgs eventArgs)
        {
            SetFeedbackText(eventArgs.Name, eventArgs.Quantity);
            ShowFeedback();
        }

        private void SetFeedbackText(string feedback, int quantity) =>
            m_FeedbackText.text = "+" + quantity + " " + feedback;
        
        private void ShowFeedback() => m_FeedbackText.gameObject.SetActive(true);
        private void HideFeedback() => m_FeedbackText.gameObject.SetActive(false);
    }
}