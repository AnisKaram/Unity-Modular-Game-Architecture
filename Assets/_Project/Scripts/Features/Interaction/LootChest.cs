using Project.Core;
using Project.Core.Interfaces;
using Project.Core.Signals;
using Project.Features.Inventory.Domain;
using UnityEngine;
using VContainer;

namespace Project.Features.Interaction
{
    public class LootChest : MonoBehaviour, IInteractable
    {
        [Header("Settings")] 
        [SerializeField] private InventoryItemSO m_LootItem;
        [SerializeField] private int m_Quantity;

        private EventBus m_EventBus;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            m_EventBus = eventBus;
        }

        public void Interact()
        {
            ItemLootedSignal signal = new ItemLootedSignal
            {
                ItemId = m_LootItem.ID,
                Quantity = m_Quantity
            };

            m_EventBus.Raise(signal);
            gameObject.SetActive(false);
            Debug.Log("Chest Opened!");
        }

        public string InteractionPrompt => "Open Chest";
    }
}