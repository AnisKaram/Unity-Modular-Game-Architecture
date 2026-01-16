using Project.Features.Inventory.Domain;
using Project.Features.Inventory.Services;
using UnityEngine;
using VContainer;

namespace Project.Features.Inventory.DebugScripts
{
    /// <summary>
    /// For testing the inventory.
    /// </summary>
    public class InventoryDebugInput : MonoBehaviour
    {
        [Header("Test Items")] 
        [SerializeField]  private InventoryItemSO m_Sword;
        [SerializeField] private InventoryItemSO m_Potion;
        
        private InventoryModel m_InventoryModel;
        private JsonInventoryService m_SaveService;

        [Inject]
        public void Construct(InventoryModel inventoryModel, JsonInventoryService saveService)
        {
            m_InventoryModel = inventoryModel;
            m_SaveService = saveService;
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.A))
            {
                // Add 1 Sword.
                Debug.Log("Adding Sword x1");
                m_InventoryModel.TryAddItem(m_Sword, 1);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // Add 5 Potions (Stacking test).
                Debug.Log("Adding Potions x5");
                m_InventoryModel.TryAddItem(m_Potion, 5);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                // Remove 1 item from Slot 0.
                Debug.Log("Removing 1 item from slot 0");
                m_InventoryModel.RemoveItem(0, 1);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                // Remove 5 items from Slot 0.
                Debug.Log("Removing 5 items from slot 0");
                m_InventoryModel.RemoveItem(0, 5);
            }
            */

            if (Input.GetKeyDown(KeyCode.K))
            {
                // For saving.
                m_SaveService.Save(m_InventoryModel);
                Debug.Log("Saved Inventory Model");
            }
        }
    }
}