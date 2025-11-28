using System;

namespace Project.Features.Inventory.Domain
{
    /// <summary>
    /// This defines a single box item in the inventory. It holds the item and how many we have.
    /// </summary>
    [Serializable]
    public class InventorySlot
    {
        public InventoryItemSO ItemData { get; private set; }
        public int Quantity { get; private set; }
        
        public bool IsEmpty => ItemData == null;

        public InventorySlot()
        {
            Clear();
        }

        public void SetItem(InventoryItemSO item, int quantity)
        {
            ItemData = item;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void Clear()
        {
            ItemData = null;
            Quantity = 0;
        }
    }
}