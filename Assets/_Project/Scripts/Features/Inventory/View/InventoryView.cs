using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project.Features.Inventory.View
{
    /// <summary>
    /// This manages the grid generation and act as the bridge between the presenter and slots.
    /// </summary>
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private InventorySlotView m_InventorySlotPrefab;
        [SerializeField] private InventoryDragGhostView m_InventoryDragGhostView;
        [SerializeField] private Transform m_Container;

        [SerializeField] private int m_DragStartIndex = -1;
        
        [SerializeField] private List<InventorySlotView> m_InventorySlots;

        public event Action<int> OnSlotClicked;
        public event Action<int, int> OnSlotDropped; // SourceIndex, DestinationIndex
        
        public void InitializeUI(int capacity)
        {
            if (capacity <= 0)
            {
                return;
            }
            
            m_InventorySlots = new List<InventorySlotView>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                InventorySlotView inventorySlotView = Instantiate(m_InventorySlotPrefab, m_Container);
                
                inventorySlotView.Initialize(i, (index) => OnSlotClicked?.Invoke(index));
                
                inventorySlotView.OnBeginDragSlot += InventorySlotView_OnBeginDragSlot;
                inventorySlotView.OnEndDragSlot += InventorySlotView_OnEndDragSlot;
                inventorySlotView.OnDropSlot += InventorySlotView_OnDropSlot;
                inventorySlotView.OnDragSlot += InventorySlotView_OnDragSlot;
                
                m_InventorySlots.Add(inventorySlotView);
            }
        }

        public void UpdateSlot(int index, Sprite icon, int quantity)
        {
            if (index < 0 || index >= m_InventorySlots.Count || 
                m_InventorySlots == null || m_InventorySlots[index] == null)
            {
                return;
            }
            
            InventorySlotView inventorySlotView = m_InventorySlots[index];
            inventorySlotView.SetData(icon, quantity);
        }
        
        public void Show() => m_Container.gameObject.SetActive(true);
        public void Hide() => m_Container.gameObject.SetActive(false);

        private void InventorySlotView_OnBeginDragSlot(int index, Sprite icon)
        {
            m_DragStartIndex = index;
            m_InventoryDragGhostView.StartDrag(icon);
        }

        private void InventorySlotView_OnEndDragSlot(int index)
        {
            m_InventoryDragGhostView.EndDrag();
            m_DragStartIndex = -1;
        }

        private void InventorySlotView_OnDropSlot(int dropIndex)
        {
            if (m_DragStartIndex != -1 && m_DragStartIndex != dropIndex)
            {
                OnSlotDropped?.Invoke(m_DragStartIndex, dropIndex);
                Debug.Log($"Item dropped, start index: {m_DragStartIndex}, drop index: {dropIndex}");
            }
        }

        private void InventorySlotView_OnDragSlot()
        {
            m_InventoryDragGhostView.UpdatePosition(Input.mousePosition);
        }
    }
}