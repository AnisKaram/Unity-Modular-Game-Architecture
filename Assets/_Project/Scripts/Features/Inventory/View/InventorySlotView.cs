using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Project.Features.Inventory.View
{
    /// <summary>
    /// This creates a wrapper for the UI inventory button prefab.
    /// </summary>
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private Button m_SlotButton;
        [SerializeField] private TextMeshProUGUI m_QuantityText;

        private void OnDestroy()
        {
            m_SlotButton.onClick.RemoveAllListeners();
        }
        
        public void Initialize(int index, Action<int> onClick)
        {
            int clickIndex = index;
            m_SlotButton.onClick.AddListener(() => onClick(clickIndex));
        }

        public void SetData(Sprite icon, int quantity)
        {
            // Hide the icon and quantity text if the icon is null
            if (icon == null)
            {
                m_Icon.gameObject.SetActive(false);
                m_QuantityText.gameObject.SetActive(false);
                return;
            }
            
            m_Icon.sprite = icon;
            m_QuantityText.text = quantity.ToString();
        }
    }
}
