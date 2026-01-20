using System;
using Project.Core;
using Project.Core.Interfaces;
using Project.Core.Signals;
using Project.Features.Inventory.Domain;
using UnityEngine;
using VContainer;

namespace Project.Features.Interaction.Components
{
    public class LootChest : MonoBehaviour, IInteractable
    {
        [Header("References")]
        [SerializeField] private LootChestAnimationEvents m_ChestEvents;
        [SerializeField] private Collider m_Collider;
        
        [Header("Settings")] 
        [SerializeField] private InventoryItemSO m_LootItem;
        [SerializeField] private int m_Quantity;
        [SerializeField] private string m_InteractionPrompt;

        private EventBus m_EventBus;

        public event Action OnOpen;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            m_EventBus = eventBus;
        }

        private void OnEnable()
        {
            m_ChestEvents.OnAnimationEnded += ChestEvents_OnAnimationEnded;
        }
        private void OnDisable()
        {
            m_ChestEvents.OnAnimationEnded -= ChestEvents_OnAnimationEnded;
        }

        public void Interact()
        {
            ItemLootedSignal signal = new ItemLootedSignal
            {
                ItemId = m_LootItem.ID,
                Quantity = m_Quantity
            };

            m_Collider.enabled = false;
            
            m_EventBus.Raise(signal);
            OnOpen?.Invoke();
        }

        public string InteractionPrompt => m_InteractionPrompt;
        
        private void ChestEvents_OnAnimationEnded()
        {
            gameObject.SetActive(false);
        }
    }
}