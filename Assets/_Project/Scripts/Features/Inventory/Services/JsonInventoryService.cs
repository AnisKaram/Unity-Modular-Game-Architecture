using System.Collections.Generic;
using Project.Features.Inventory.Data;
using Project.Features.Inventory.Domain;
using UnityEngine;
using System.IO;

namespace Project.Features.Inventory.Services
{
    public class JsonInventoryService
    {
        private readonly string m_FilePath;
        private readonly ItemRegistry m_ItemRegistry;

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonInventoryService(ItemRegistry itemRegistry)
        {
            m_ItemRegistry = itemRegistry;
            m_FilePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        }


        /// <summary>
        /// Save inventory items.
        /// </summary>
        public void Save(InventoryModel model)
        {
            // Create a new InventorySaveData object.
            InventorySaveData saveData = new InventorySaveData
            {
                inventoryItems = new List<SlotSaveData>()
            };

            // Loop through all the slots in the InventoryModel.
            for (int index = 0; index < model.Slots.Count; index++)
            {
                InventorySlot slot = model.Slots[index];

                // Continue/Don't save if the slot is empty
                if (slot.IsEmpty)
                {
                    continue;
                }

                // Create a new SlotSaveData struct and assign the values.
                SlotSaveData slotSaveData = new SlotSaveData
                {
                    itemID = slot.ItemData.ID,
                    quantity = slot.Quantity,
                    slotIndex = index
                };

                // Add the created struct to the list of structs in the InventorySaveData
                saveData.inventoryItems.Add(slotSaveData);
            }

            // Save the items in a Json file.
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(m_FilePath, json);
            Debug.Log("Data saved under this path: " + m_FilePath);
        }

        /// <summary>
        /// Load and set inventory items.
        /// </summary>
        public void Load(InventoryModel model)
        {
            // Return if the file doesn't exist.
            if (!File.Exists(m_FilePath))
            {
                Debug.Log("File not found: " + m_FilePath);
                return;
            }

            // Read the Json file and convert it into a InventorySaveData object
            string json = File.ReadAllText(m_FilePath);
            InventorySaveData saveData = JsonUtility.FromJson<InventorySaveData>(json);
            
            // Clear all the slots in the model.
            model.ClearAllSlots();

            // Loop through all the saved inventory items and set them to the model if possible.
            foreach (var data in saveData.inventoryItems)
            {
                InventoryItemSO inventoryItem = m_ItemRegistry.GetItem(data.itemID);

                if (inventoryItem == null)
                {
                    continue;
                }
                
                model.GetSlot(data.slotIndex).SetItem(inventoryItem, data.quantity);
            }
        }
    }
}