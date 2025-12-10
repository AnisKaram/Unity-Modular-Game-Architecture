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
        public NavMeshAgent Agent { get; private set; }
        public InventoryModel Inventory { get; private set; }
        public Transform Target { get; private set; }

        [Inject]
        public void Construct(PlayerController playerController)
        {
            Target = playerController.transform;
        }

        private void Awake()
        {
            // Get the NavMeshAgent component.
            if (transform.TryGetComponent(out NavMeshAgent agent))
            {
                Agent = agent;
            }

            // Small bag for the NPC.
            Inventory = new InventoryModel(5);
        }
    }
}