using System;
using Project.Features.Inventory.Domain;
using UnityEngine;
using VContainer.Unity;

namespace Project.Features.Inventory.Presentation
{
    public class InventoryPresenter : IStartable, IDisposable
    {
        private readonly InventoryModel m_InventoryModel;

        public InventoryPresenter(InventoryModel inventoryModel)
        {
            m_InventoryModel = inventoryModel;
        }
        
        public void Start()
        {
            m_InventoryModel.OnSlotUpdated += InventoryModel_OnSlotUpdated;
            Debug.Log("Starting InventoryPresenter");
        }
        public void Dispose()
        {
            m_InventoryModel.OnSlotUpdated -= InventoryModel_OnSlotUpdated;
            Debug.Log("Destroying InventoryPresenter");
        }
        
        private void InventoryModel_OnSlotUpdated(int slot)
        {
            Debug.Log("Slot Updated, index: " + slot);
        }
    }
}