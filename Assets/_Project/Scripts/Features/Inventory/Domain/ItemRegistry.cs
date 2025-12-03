using System.Collections.Generic;
using UnityEngine;

namespace Project.Features.Inventory.Domain
{
    [CreateAssetMenu(fileName = "New Item Registry", menuName = "Inventory/Item Registry")]
    public class ItemRegistry : ScriptableObject
    {
        [Header("Items")]
        [SerializeField] private List<InventoryItemSO> m_InventoryItems;
        
        private Dictionary<string, InventoryItemSO> m_InventoryDictionary;

        private void OnEnable()
        {
            if (m_InventoryItems == null)
            {
                Debug.LogError("List of Inventory items is null");
                return;
            }
            
            m_InventoryDictionary = new Dictionary<string, InventoryItemSO>();
            
            foreach (InventoryItemSO item in m_InventoryItems)
            {
                bool isItemAdded = m_InventoryDictionary.TryAdd(item.ID, item);
                Debug.Log("Item: " + item.ID + " with name: " + item.Name + " is added to Inventory: " + isItemAdded);
            }
        }

        public InventoryItemSO GetItem(string id)
        {
            if (m_InventoryItems == null)
            {
                Debug.LogError("List of Inventory items is null");
                return null;
            }
            
            if (m_InventoryDictionary == null)
            {
                Debug.LogError("Dictionary of Inventory items is null");
                return null;
            }

            bool isFetched = m_InventoryDictionary.TryGetValue(id, out InventoryItemSO item);
            return isFetched ? item : null;
        }
    }
}