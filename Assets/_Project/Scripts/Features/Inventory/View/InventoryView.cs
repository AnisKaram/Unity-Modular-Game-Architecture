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
        [SerializeField] private Transform m_Container;
        
        [SerializeField] private List<InventorySlotView> m_InventorySlots;

        public event Action<int> OnSlotClicked;

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
                inventorySlotView.Initialize(i, OnSlotClicked);
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
    }
}