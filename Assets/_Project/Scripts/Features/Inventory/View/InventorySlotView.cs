using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Project.Features.Inventory.View
{
    /// <summary>
    /// This creates a wrapper for the UI inventory button prefab.
    /// </summary>
    public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private Image m_Icon;
        [SerializeField] private Image m_EmptyIcon;
        [SerializeField] private Button m_SlotButton;
        [SerializeField] private TextMeshProUGUI m_QuantityText;
        [SerializeField] private int m_Index;
        
        public event Action<int, Sprite> OnBeginDragSlot;
        public event Action<int> OnEndDragSlot;
        public event Action<int> OnDropSlot;
        public event Action OnDragSlot;
        
        
        private void OnDestroy()
        {
            m_SlotButton.onClick.RemoveAllListeners();

            OnBeginDragSlot = null;
            OnEndDragSlot = null;
            OnDropSlot = null;
            OnDragSlot = null;
        }
        
        public void Initialize(int index, Action<int> onClick)
        {
            int clickIndex = index;
            m_Index = clickIndex;
            m_SlotButton.onClick.AddListener(() => onClick(clickIndex));
        }

        public void SetData(Sprite icon, int quantity)
        {
            // Empty Slot
            if (icon == null)
            {
                m_EmptyIcon.gameObject.SetActive(true);
                m_Icon.gameObject.SetActive(false);
                m_QuantityText.gameObject.SetActive(false);
                return;
            }
            
            // Valid item
            m_EmptyIcon.gameObject.SetActive(false);
            m_Icon.gameObject.SetActive(true);
            m_Icon.sprite = icon;
            
            // Show the text if quantity is 2+ 
            bool isStack = quantity > 1;
            m_QuantityText.gameObject.SetActive(isStack);
            if (isStack)
            {
                m_QuantityText.text = quantity.ToString();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Only start the drag if the icon is active
            if (!m_Icon.gameObject.activeSelf)
            {
                return;
            }
            OnBeginDragSlot?.Invoke(m_Index, GetIcon());
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragSlot?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragSlot?.Invoke(m_Index);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropSlot?.Invoke(m_Index);
        }

        private Sprite GetIcon() => m_Icon.sprite;
    }
}