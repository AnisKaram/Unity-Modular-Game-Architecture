using System;
using Project.Features.Inventory.Domain;
using Project.Features.Inventory.View;
using UnityEngine;
using VContainer.Unity;

namespace Project.Features.Inventory.Presentation
{
    public class InventoryPresenter : IStartable, IDisposable
    {
        private readonly InventoryModel m_InventoryModel;
        private readonly InventoryView m_InventoryView;
        
        public InventoryPresenter(InventoryModel inventoryModel, InventoryView inventoryView)
        {
            m_InventoryModel = inventoryModel;
            m_InventoryView = inventoryView;
        }
        
        public void Start()
        {
            Debug.Log("Starting InventoryPresenter");
            
            m_InventoryModel.OnSlotUpdated += InventoryModel_OnSlotUpdated;
            m_InventoryView.OnSlotClicked += InventoryView_OnSlotClicked;
            
            // Initialize the Grid
            int inventoryCapacity = m_InventoryModel.Capacity;
            m_InventoryView.InitializeUI(inventoryCapacity);
            
            // Update the grid if model has saved inventory items
            for (int i = 0; i < inventoryCapacity; i++)
            {
                HandleSlotUpdate(i);
            }
        }
        public void Dispose()
        {
            Debug.Log("Destroying InventoryPresenter");
            
            m_InventoryModel.OnSlotUpdated -= InventoryModel_OnSlotUpdated;
            m_InventoryView.OnSlotClicked -= InventoryView_OnSlotClicked;
        }
        
        private void InventoryModel_OnSlotUpdated(int slotIndex)
        {
            Debug.Log("Slot Updated, index: " + slotIndex);
            
            HandleSlotUpdate(slotIndex);
        }
        
        private void InventoryView_OnSlotClicked(int slotIndex)
        {
            Debug.Log("Slot Clicked, index: " + slotIndex);
        }

        private void HandleSlotUpdate(int index)
        {
            InventorySlot slot = m_InventoryModel.GetSlot(index);

            if (slot.IsEmpty)
            {
                m_InventoryView.UpdateSlot(index, null, 0);
            }
            else
            {
                m_InventoryView.UpdateSlot(index, slot.ItemData.Icon, slot.Quantity);
            }
        }
    }
}