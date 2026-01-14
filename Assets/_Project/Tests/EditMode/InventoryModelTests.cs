using NUnit.Framework;
using Project.Features.Inventory.Domain;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Project.Tests
{
    public class InventoryModelTests
    {
        private InventoryModel m_InventoryModel;
        private InventoryItemSO m_PotionItem;
        private InventoryItemSO m_SwordItem;

        // Since it has Set up annotation, it will run with every test
        // This is first step called: Arrange (Setup).
        [SetUp]
        public void Setup()
        {
            // Arrange.
            m_InventoryModel = new InventoryModel(10);

            m_PotionItem = ScriptableObject.CreateInstance<InventoryItemSO>();
            m_PotionItem.SetID("Potion_ID");
            m_PotionItem.name = "Potion";
            m_PotionItem.SetMaxStack(3);

            m_SwordItem = ScriptableObject.CreateInstance<InventoryItemSO>();
            m_SwordItem.SetID("Sword_ID");
            m_SwordItem.name = "Sword";
            m_SwordItem.SetMaxStack(5);
        }

        // The Goal: Prove that adding an item to an empty inventory works.
        [Test]
        public void AddNewItem_InventoryEmpty()
        {
            // Act
            bool result = m_InventoryModel.TryAddItem(m_PotionItem, 1);

            // Assert
            Assert.IsTrue(result, "TryAddItem should return true");
            Assert.AreEqual(1, m_InventoryModel.GetSlot(0).Quantity, "Slot 0 should return 1");
            Assert.AreEqual(m_PotionItem, m_InventoryModel.GetSlot(0).ItemData, "Slot 0 should hold item data: Potion");
        }

        [Test]
        public void AddItemMoreThanMaxStack_InventoryEmpty()
        {
            // Act
            bool sword_1 = m_InventoryModel.TryAddItem(m_SwordItem, 10); // slot 0 and 1
            bool sword_2 = m_InventoryModel.TryAddItem(m_SwordItem, 10); // slot 2 and 3

            // Assert
            Assert.IsTrue(sword_1, "TryAddItem: Sword_1 should return true");
            Assert.IsTrue(sword_2, "TryAddItem: Sword_2 should return true");

            Assert.AreEqual(5, m_InventoryModel.GetSlot(0).Quantity, "Slot 0 should return 5");
            Assert.AreEqual(5, m_InventoryModel.GetSlot(1).Quantity, "Slot 1 should return 5");
            Assert.AreEqual(5, m_InventoryModel.GetSlot(2).Quantity, "Slot 2 should return 5");
            Assert.AreEqual(5, m_InventoryModel.GetSlot(3).Quantity, "Slot 3 should return 5");
        }

        // Test that adding the same item twice combines them, rather than taking a new slot.
        [Test]
        public void AddSameItemTwice_InventoryNotEmpty()
        {
            // Act
            bool potion_1 = m_InventoryModel.TryAddItem(m_PotionItem, 1);
            bool potion_2 = m_InventoryModel.TryAddItem(m_PotionItem, 2);

            // Assert
            Assert.IsTrue(potion_1, "TryAddItem potion_1 should return true");
            Assert.IsTrue(potion_2, "TryAddItem potion_2 should return true");

            Assert.AreEqual(3, m_InventoryModel.GetSlot(0).Quantity, "Slot 0 should return 3");
            Assert.IsTrue(m_InventoryModel.GetSlot(1).IsEmpty, "Slot 1 should return true");
        }

        // Test that removing the exact amount clears the slot completely.
        [Test]
        public void RemoveExactAmountItem_InventoryNotEmpty()
        {
            // Act
            bool potion_1 = m_InventoryModel.TryAddItem(m_PotionItem, 3);

            // Assert
            Assert.IsTrue(potion_1, "TryAddItem potion_1 should return true");

            // Act
            m_InventoryModel.RemoveItem(0, 3);

            // Assert
            Assert.IsTrue(m_InventoryModel.GetSlot(0).IsEmpty, "Slot 0 should be empty after removing 3 from index 0");
            Assert.AreEqual(0, m_InventoryModel.GetSlot(0).Quantity, "Slot 0 should return 0");
        }

        // Test Swapping two items.
        [Test]
        public void SwapItems_InventoryNotEmpty()
        {
            // Arrange
            bool potionResult = m_InventoryModel.TryAddItem(m_PotionItem, 1);
            bool swordResult = m_InventoryModel.TryAddItem(m_SwordItem, 1);

            Assert.AreEqual(m_PotionItem, m_InventoryModel.GetSlot(0).ItemData, "Slot 0 should hold item data: Potion");
            Assert.AreEqual(m_SwordItem, m_InventoryModel.GetSlot(1).ItemData, "Slot 1 should hold item data: Sword");
            Assert.IsTrue(potionResult, "TryAddItem: Potion return true");
            Assert.IsTrue(swordResult, "TryAddItem: Sword should return true");

            // Act
            m_InventoryModel.SwapSlots(0, 1);

            Assert.AreEqual(1, m_InventoryModel.GetItemIndex("Potion_ID"), "Potion index should be 1");
            Assert.AreEqual(0, m_InventoryModel.GetItemIndex("Sword_ID"), "Sword index should be 0");
            Assert.AreEqual(m_SwordItem, m_InventoryModel.GetSlot(0).ItemData, "Slot 0 should hold sword");
            Assert.AreEqual(m_PotionItem, m_InventoryModel.GetSlot(1).ItemData, "Slot 1 should hold potion");
        }
    }
}