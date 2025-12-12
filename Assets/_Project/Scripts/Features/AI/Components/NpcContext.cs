using Project.Features.Character;
using Project.Features.Inventory.Domain;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace Project.Features.AI.Components
{
    /// <summary>
    /// Represents the ToolBox for the Actions.
    /// </summary>
    public class NpcContext : MonoBehaviour
    {
        [SerializeField] private InventoryItemSO m_Apple;
        
        public NavMeshAgent Agent { get; private set; }
        public InventoryModel Inventory { get; private set; }
        public Transform Target { get; private set; }
        public NpcStats Stats { get; private set; }

        [Inject]
        public void Construct(PlayerController playerController)
        {
            Target = playerController.transform;
        }

        private void Awake()
        {
            // Get the components.
            if (transform.TryGetComponent(out NavMeshAgent agent))
            {
                Agent = agent;
            }

            if (transform.TryGetComponent(out NpcStats stats))
            {
                Stats = stats;
            }

            // Small bag for the NPC.
            Inventory = new InventoryModel(5);

            if (m_Apple != null)
            {
                Inventory.TryAddItem(m_Apple, 1);
            }
        }
        
        public InventoryItemSO GetApple() => m_Apple;
    }
}