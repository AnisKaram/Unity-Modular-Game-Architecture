using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project.Features.Inventory.Domain
{
    /// <summary>
    /// Manages the list of slots (InventorySlot), the rules if we can add/remove/swap/full.
    /// </summary>
    public class InventoryModel
    {
        public List<InventorySlot> Slots { get; private set; }
        public int Capacity { get; private set; }

        public event Action<int> OnSlotUpdated;

        // Constructor.
        public InventoryModel(int capacity)
        {
            Capacity = capacity;
            Slots = new List<InventorySlot>();

            for (int i = 0; i < Capacity; i++)
            {
                Slots.Add(new InventorySlot());
            }
        }

        // Try adding a new item to the Slots list or add it to the existing and the leftover (if-any).
        public bool TryAddItem(InventoryItemSO itemToAdd, int quantity)
        {
            // First, we need to try to find an existing item in the Slots list.
            foreach (InventorySlot slot in Slots)
            {
                if (!slot.IsEmpty && slot.ItemData.ID == itemToAdd.ID) // Found an existing item in the Inventory
                {
                    int space = itemToAdd.MaxStack - slot.Quantity; // How much space is left in this specific slot?

                    if (space > 0)
                    {
                        int quantityToAdd = Mathf.Min(space, quantity); // Add as much as the slot can take

                        slot.AddQuantity(quantityToAdd);

                        quantity -= quantityToAdd;

                        OnSlotUpdated?.Invoke(Slots.IndexOf(slot));

                        if (quantity <= 0) // We placed everything 
                        {
                            return true;
                        }
                    }
                }
            }
            
            // Second, we need to try to find an empty slot and add it to it.
            foreach (InventorySlot slot in Slots)
            {
                if (quantity <= 0)
                {
                    break;
                }

                if (slot.IsEmpty)
                {
                    int space = itemToAdd.MaxStack;
                    
                    if (space > 0)
                    {
                        int quantityToAdd = Mathf.Min(space, quantity);
                        slot.SetItem(itemToAdd, quantityToAdd); // Put the rest here
                        quantity -= quantityToAdd;
                        
                        OnSlotUpdated?.Invoke(Slots.IndexOf(slot));
                        
                        if (quantity <= 0)
                        {
                            return true;
                        }   
                    }
                }
            }

            // Inventory is full
            return quantity <= 0;
        }

        // Remove an item from the Slots list.
        public void RemoveItem(int index, int amount)
        {
            InventorySlot slot = GetSlot(index);

            if (slot == null || slot.IsEmpty)
            {
                return;
            }

            int amountLeft = slot.Quantity - amount;

            if (amountLeft <= 0)
            {
                slot.Clear();
            }
            else
            {
                slot.SetItem(slot.ItemData, amountLeft);
            }

            OnSlotUpdated?.Invoke(index);
        }

        // Swap two items between two slots.
        public void SwapSlots(int indexA, int indexB)
        {
            InventorySlot slotA = GetSlot(indexA);
            InventorySlot slotB = GetSlot(indexB);

            if (slotA == null || slotB == null)
            {
                return;
            }

            // Store A in temporary fields.
            var tempItem = slotA.ItemData;
            var tempQuantity = slotA.Quantity;
            
            // Swap B into A
            slotA.SetItem(slotB.ItemData, slotB.Quantity);
            
            // Move temp A into B.
            slotB.SetItem(tempItem, tempQuantity);

            // Notify both slots that has been updated.
            OnSlotUpdated?.Invoke(indexA);
            OnSlotUpdated?.Invoke(indexB);
        }

        // Get the total quantity of an item in the Slots list.
        public int GetTotalQuantity(string itemID)
        {
            int total = 0;
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];
                if (!slot.IsEmpty && slot.ItemData.ID == itemID)
                {
                    total += slot.Quantity;
                }
            }

            return total;
        }

        public int GetItemIndex(string itemID)
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];
                if (slot.ItemData.ID == itemID)
                {
                    return i;
                }
            }

            return -1;
        }

        // Use and consume the item.
        public bool ConsumeItem(string itemID, int amount)
        {
            if (GetTotalQuantity(itemID) < amount)
            {
                return false;
            }

            // Remove from multiple slots if available.
            for (int i = 0; i < Slots.Count; i++)
            {
                if (amount <= 0)
                {
                    break;
                }

                var slot = Slots[i];
                if (!slot.IsEmpty && slot.ItemData.ID == itemID)
                {
                    int amountToRemove = Mathf.Min(slot.Quantity, amount);

                    RemoveItem(i, amountToRemove);

                    amount -= amountToRemove;
                }
            }

            return true;
        }

        // Checks if the inventory is full or not.
        public bool IsFull()
        {
            for (int i = 0; i < Slots.Count; i++)
            {
                var slot = Slots[i];
                if (slot.IsEmpty)
                {
                    return false;
                }
            }

            return true;
        }

        // Get the slot from the Slots list or null if not found.
        public InventorySlot GetSlot(int index)
        {
            if (index >= 0 && index < Slots.Count)
            {
                return Slots[index];
            }

            return null;
        }

        public void ClearAllSlots()
        {
            foreach (InventorySlot slot in Slots)
            {
                slot.Clear();
            }
        }
    }
}